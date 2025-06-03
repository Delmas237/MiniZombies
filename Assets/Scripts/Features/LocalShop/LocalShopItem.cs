using System;
using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopItem
    {
        [SerializeField] private string _name;
        [SerializeField] private int _price;
        [SerializeField] private TextMeshProUGUI _priceText;

        public string Name => _name;
        public int Price => _price;
        public TextMeshProUGUI PriceText => _priceText;
    }
}
