using System;
using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopWeapon : LocalShopItem
    {
        [Space(10)]
        [SerializeField] private int _priceLvlBoost;
        [SerializeField] private int _damageLvlBoost;
        [SerializeField] private TextMeshProUGUI _damageText;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _lvlText;

        public int PriceLvlBoost => _priceLvlBoost;
        public int DamageLvlBoost => _damageLvlBoost;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI LvlText => _lvlText;
    }
}
