using System;
using UnityEngine;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerCurrencyModule : IPlayerCurrencyModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] protected int _coins;
        
        public event Action<int> CoinsChanged;
        
        public bool Enabled { get => _enabled; set => _enabled = value; }
        public int Coins => _coins;

        public void Add(int amount)
        {
            if (!_enabled)
                return;

            if (amount <= 0)
                return;

            _coins += amount;
            CoinsChanged?.Invoke(_coins);
        }

        public bool Spend(int amount)
        {
            if (!_enabled)
                return false;

            if (amount < 0 || amount > _coins)
                return false;

            _coins -= amount;
            CoinsChanged?.Invoke(_coins);
            return true;
        }

        public bool IsCanSpend(int amount) => _enabled && _coins >= amount;
    }
}
