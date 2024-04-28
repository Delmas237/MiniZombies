using UnityEngine;

public static class Bank
{
    private static int coins;
    public static int Coins
    {
        get { return coins; }
        set
        {
            coins = Mathf.Clamp(value, 0, MaxCoins);
            Save();
        }
    }

    public const int MaxCoins = 999999;

    public static void Load()
    {
        Coins = PlayerPrefs.GetInt(nameof(coins));
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(nameof(coins), coins);
        PlayerPrefs.Save();
    }
}
