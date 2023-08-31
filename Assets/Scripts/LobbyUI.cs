using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI maxWaveText;

    private void Awake()
    {
        Bank.Load();
        maxWaveText.text = PlayerPrefs.GetInt("MaxWave").ToString();
    }

    private void Update()
    {
        coinsText.text = Bank.Coins.ToString();
    }

    public void Play() => SceneManager.LoadScene("Game");
}
