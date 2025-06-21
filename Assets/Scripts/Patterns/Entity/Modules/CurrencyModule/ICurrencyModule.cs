using System;

namespace PlayerLib
{
    public interface ICurrencyModule
    {
        public event Action<int> CoinsChanged;

        public int Coins { get; }

        public void Add(int amount);
        public bool Spend(int amount);
        public bool IsCanSpend(int amount);
    }
}
