using System;

namespace Entity.Friendly.Player
{
    public interface IPlayerCurrencyModule : IModule
    {
        event Action<int> CoinsChanged;

        int Coins { get; }

        void Add(int amount);
        bool Spend(int amount);
        bool IsCanSpend(int amount);
    }
}
