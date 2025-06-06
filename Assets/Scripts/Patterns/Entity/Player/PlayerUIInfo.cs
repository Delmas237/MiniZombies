using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerLib
{
    public class PlayerUIInfo : MonoBehaviour
    {
        [SerializeField] private PlayerContainer _player;

        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _bullets;
        [SerializeField] private TextMeshProUGUI _coins;

        private void Start()
        {
            if (_player != null)
            {
                UpdateHealthBar();
                UpdateCoinsText(_player.CurrencyModule.Coins);
                UpdateBulletsText(_player.WeaponsModule.Bullets);

                _player.HealthModule.Increased += UpdateHealthBar;
                _player.HealthModule.Decreased += UpdateHealthBar;

                _player.CurrencyModule.CoinsChanged += UpdateCoinsText;
                _player.WeaponsModule.BulletsChanged += UpdateBulletsText;
            }
        }

        private void UpdateHealthBar()
        {
            _healthBar.fillAmount = _player.HealthModule.Health / _player.HealthModule.MaxHealth;
        }

        private void UpdateCoinsText(int amount)
        {
            _coins.text = amount.ToString();
        }

        private void UpdateBulletsText(int amount)
        {
            _bullets.text = amount.ToString();
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.HealthModule.Increased -= UpdateHealthBar;
                _player.HealthModule.Decreased -= UpdateHealthBar;

                _player.CurrencyModule.CoinsChanged -= UpdateCoinsText;
                _player.WeaponsModule.BulletsChanged -= UpdateBulletsText;
            }
        }
    }
}
