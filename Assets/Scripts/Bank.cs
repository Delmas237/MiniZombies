using UnityEngine;

public static class Bank
{
    private static int coins;
    public static int Coins
    {
        get { return coins; }
        set
        {
            if (value < 0)
                coins = 0;
            else if (value > MaxCoins)
                coins = MaxCoins;
            else
                coins = value;

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
