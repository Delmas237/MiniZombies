using System;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopItemData
    {
        [SerializeField] private GunType _type;
        [SerializeField] private GlobalShopParameterData _damageParameter;
        [SerializeField] private GlobalShopParameterData _cooldownParameter;
        [SerializeField] private GlobalShopParameterData _distanceParameter;

        public GunType Type => _type;
        public GlobalShopParameterData DamageParameter => _damageParameter;
        public GlobalShopParameterData CooldownParameter => _cooldownParameter;
        public GlobalShopParameterData DistanceParameter => _distanceParameter;
    }
}
