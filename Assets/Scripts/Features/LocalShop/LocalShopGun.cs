using System;
using TMPro;
using UnityEngine;
using Weapons;

namespace LocalShopLib
{
    [Serializable]
    public class LocalShopGun
    {
        [SerializeField] private GunType _type;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _lvlText;

        public GunType Type => _type;
        public TextMeshProUGUI PriceText => _priceText;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI LvlText => _lvlText;
    }
}
