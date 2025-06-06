using System;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class CurrencyModule : ICurrencyModule
    {
        [SerializeField] protected int _coins;
        
        public event Action<int> CoinsChanged;
        
        public int Coins => _coins;

        public void Add(int amount)
        {
            if (amount <= 0)
                return;

            _coins += amount;
            CoinsChanged?.Invoke(_coins);
        }

        public bool Spend(int amount)
        {
            if (amount < 0 || amount > _coins)
                return false;

            _coins -= amount;
            CoinsChanged?.Invoke(_coins);
            return true;
        }
    }
}
