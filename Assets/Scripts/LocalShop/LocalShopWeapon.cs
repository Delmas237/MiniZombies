using TMPro;
using UnityEngine;

namespace LocalShopLib
{
    public class LocalShopWeapon : LocalShopItem
    {
        [Space(10)]
        public int DamageLvlBoost;
        public TextMeshProUGUI DamageText;
        [Space(10)]
        public TextMeshProUGUI LvlText;
    }
}
