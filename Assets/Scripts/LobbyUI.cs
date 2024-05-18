using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _maxWaveText;

    private void Awake()
    {
        Bank.Load();
        _maxWaveText.text = PlayerPrefs.GetInt("MaxWave").ToString();
        
        UpdateText(Bank.Coins);
        Bank.CoinsChanged += UpdateText;
    }

    private void UpdateText(int amount)
    {
        _coinsText.text = amount.ToString();
    }

    public void Play() => SceneManager.LoadScene("Game");
}
