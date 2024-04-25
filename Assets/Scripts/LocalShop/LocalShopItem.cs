using System;
using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopItem
    {
        public string Name;
        public int Price;
        public TextMeshProUGUI PriceText;
    }
}
