using System;
using UnityEngine;

public static class Bank
{
    private static int _coins;
    public static int Coins
    {
        get 
        {
            Load();
            return _coins; 
        }
        set
        {
            _coins = Mathf.Clamp(value, 0, MaxCoins);
            CoinsChanged?.Invoke(_coins);
            Save();
        }
    }
    public static event Action<int> CoinsChanged;

    public const int MaxCoins = 999999;

    private static bool _initialized;

    public static void Load()
    {
        if (_initialized)
            return;

        Coins = PlayerPrefs.GetInt(nameof(_coins));
        _initialized = true;
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(nameof(_coins), _coins);
        PlayerPrefs.Save();
    }
}
