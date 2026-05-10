using System;

namespace PlayerLib
{
    public interface ICurrencyModule
    {
        event Action<int> CoinsChanged;

        int Coins { get; }

        void Add(int amount);
        bool Spend(int amount);
        bool IsCanSpend(int amount);
    }
}
