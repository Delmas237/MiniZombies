using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopParameter
    {
        public GlobalShopValue Price;
        public GlobalShopValue Up;
        public GlobalShopValue Info;
        [Space(10)]
        public Button PurchaseButton;
        public event Action Purchased;

        public void Initialize()
        {
            PurchaseButton.onClick.AddListener(Purchase);
        }

        public void Purchase()
        {
            if (Bank.Spend(Mathf.FloorToInt(Price.Value)) && Info.Value > Up.Value)
            {
                Purchased.Invoke();
                UpdateText();
            }
        }

        public void UpdateText()
        {
            Price.Text.text = Price.Value.ToString() + "$";
            Up.Text.text = Up.Value.ToString();

            Info.Value = (float)Math.Round(Info.Value, 2);
            Info.Text.text = Info.Value.ToString();
        }
    }

    [Serializable]
    public class GlobalShopValue
    {
        public float Value;
        public TextMeshProUGUI Text;
    }
}
