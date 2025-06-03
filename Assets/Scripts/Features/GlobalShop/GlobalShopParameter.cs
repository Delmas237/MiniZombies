using System;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopParameter
    {
        [SerializeField] private GlobalShopValue _price;
        [SerializeField] private GlobalShopValue _up;
        [SerializeField] private GlobalShopValue _info;
        [Space(10)]
        [SerializeField] private Button _purchaseButton;
        public event Action Purchased;

        public GlobalShopValue Price => _price;
        public GlobalShopValue Up => _up;
        public GlobalShopValue Info => _info;
        public Button PurchaseButton => _purchaseButton;

        public void Initialize()
        {
            _purchaseButton.onClick.AddListener(Purchase);
        }

        public void Purchase()
        {
            if (_info.Value > _up.Value && Bank.Spend(Mathf.FloorToInt(_price.Value)))
            {
                Purchased.Invoke();
                UpdateText();
            }
        }

        public void UpdateText()
        {
            _price.Text.text = _price.Value.ToString() + "$";
            _up.Text.text = _up.Value.ToString();

            _info.Value = (float)Math.Round(_info.Value, 2);
            _info.Text.text = _info.Value.ToString();
        }
    }
}
