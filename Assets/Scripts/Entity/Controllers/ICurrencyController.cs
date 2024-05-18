using System;

namespace PlayerLib
{
    public interface ICurrencyController
    {
        public int Coins { get; }
        public event Action<int> CoinsChanged;

        public void Add(int amount);
        public void Spend(int amount);
    }
}
