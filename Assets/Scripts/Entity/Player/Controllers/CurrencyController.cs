using System;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class CurrencyController : ICurrencyController
    {
        [SerializeField] protected int _coins;
        public int Coins => _coins;

        public void Add(int amount)
        {
            if (amount <= 0)
                return;

            _coins += amount;
        }

        public void Spend(int amount)
        {
            if (amount >= 0)
                return;

            _coins -= amount;
        }
    }
}
