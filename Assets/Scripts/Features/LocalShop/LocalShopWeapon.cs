using System;
using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopWeapon : LocalShopItem
    {
        [Space(10)]
        public int PriceLvlBoost;
        public int DamageLvlBoost;
        public TextMeshProUGUI DamageText;
        [Space(10)]
        public TextMeshProUGUI LvlText;
    }
}
