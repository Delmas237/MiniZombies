using System;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopItemData
    {
        [SerializeField] private string _name;
        [SerializeField] private int _price;

        public string Name => _name;
        public int Price => _price;
    }
}
