using System;
using TMPro;
using UnityEngine;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopValue
    {
        [SerializeField] private TextMeshProUGUI _text;

        [field: SerializeField] public float Value { get; set; }
        public TextMeshProUGUI Text => _text;
    }
}