using System;
using TMPro;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShopParameter : MonoBehaviour
    {
        [field: SerializeField] public int Price { get; set; }
        [field: SerializeField] public float Up { get; set; }
        [field: SerializeField] public float Info { get; set; }

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI upText;
        [SerializeField] private TextMeshProUGUI infoText;

        public event Action Purchased;

        public void Purchase()
        {
            if (Bank.Coins >= Price && Info > Up)
            {
                Bank.Coins -= Price;

                Purchased.Invoke();

                UpdateText();
            }
        }

        public void UpdateText()
        {
            priceText.text = Price.ToString() + "$";
            upText.text = Up.ToString();

            Info = (float)Math.Round(Info, 2);
            infoText.text = Info.ToString();
        }
    }
}
