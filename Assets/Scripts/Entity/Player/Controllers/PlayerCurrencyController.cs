using System;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class PlayerCurrencyController
    {
        [field: SerializeField] public int Coins { get; set; } = 0;
    }
}
