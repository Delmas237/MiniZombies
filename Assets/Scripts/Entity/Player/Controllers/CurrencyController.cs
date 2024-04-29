using System;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class CurrencyController
    {
        [field: SerializeField] public int Coins { get; set; } = 0;
    }
}
