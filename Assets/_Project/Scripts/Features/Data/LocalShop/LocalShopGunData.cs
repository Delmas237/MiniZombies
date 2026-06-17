using System;
using UnityEngine;
using Weapons;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopGunData
    {
        [SerializeField] private GunType _type;
        [SerializeField] private int _price;
        [Space(10)]
        [SerializeField] private int _priceLvlBoost;
        [SerializeField] private int _damageLvlBoost;

        public GunType Type => _type;
        public int Price => _price;
        public int PriceLvlBoost => _priceLvlBoost;
        public int DamageLvlBoost => _damageLvlBoost;
    }
}
