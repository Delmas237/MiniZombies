using System;
using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopItem
    {
        [SerializeField] private string _name;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _priceText;

        public string Name => _name;
        public TextMeshProUGUI PriceText => _priceText;
    }
}
