using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopParameter
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _upText;
        [SerializeField] private TextMeshProUGUI _infoText;
        [Space(10)]
        [SerializeField] private Button _purchaseButton;

        public TextMeshProUGUI PriceText => _priceText;
        public TextMeshProUGUI UpText => _upText;
        public TextMeshProUGUI InfoText => _infoText;
        public Button PurchaseButton => _purchaseButton;
    }
}
