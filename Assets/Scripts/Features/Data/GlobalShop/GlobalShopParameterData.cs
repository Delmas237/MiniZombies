using System;
using UnityEngine;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopParameterData
    {
        [SerializeField] private float _price;
        [SerializeField] private float _up;

        public float Price => _price;
        public float Up => _up;
    }
}
