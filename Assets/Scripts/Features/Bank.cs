using System;
using UnityEngine;

public static class Bank
{
    private static bool _initialized;
    private static int _coins;

    public static event Action<int> CoinsChanged;

    public static int Coins
    {
        get
        {
            if (!_initialized)
                Load();
            return _coins;
        }
    }

    private static void Load()
    {
        _coins = PlayerPrefs.GetInt(nameof(_coins));
        _initialized = true;
    }

    private static void Save()
    {
        PlayerPrefs.SetInt(nameof(_coins), _coins);
        PlayerPrefs.Save();
    }

    public static void Add(int amount)
    {
        if (!_initialized)
            Load();

        if (amount <= 0)
            return;

        _coins += amount;
        CoinsChanged?.Invoke(_coins);
        Save();
    }

    public static bool Spend(int amount)
    {
        if (!_initialized)
            Load();

        if (amount < 0 || amount > _coins)
            return false;

        _coins -= amount;
        CoinsChanged?.Invoke(_coins);
        Save();
        return true;
    }
}
