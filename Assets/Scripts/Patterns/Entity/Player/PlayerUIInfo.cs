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
                UpdateCoinsText(_player.CurrencyController.Coins);
                UpdateBulletsText(_player.WeaponsController.Bullets);

                _player.HealthController.Healed += UpdateHealthBar;
                _player.HealthController.Damaged += UpdateHealthBar;

                _player.CurrencyController.CoinsChanged += UpdateCoinsText;
                _player.WeaponsController.BulletsChanged += UpdateBulletsText;
            }
        }
        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.HealthController.Healed -= UpdateHealthBar;
                _player.HealthController.Damaged -= UpdateHealthBar;

                _player.CurrencyController.CoinsChanged -= UpdateCoinsText;
                _player.WeaponsController.BulletsChanged -= UpdateBulletsText;
            }
        }

        private void UpdateHealthBar()
        {
            _healthBar.fillAmount = _player.HealthController.Health / _player.HealthController.MaxHealth;
        }

        private void UpdateCoinsText(int amount)
        {
            _coins.text = amount.ToString();
        }

        private void UpdateBulletsText(int amount)
        {
            _bullets.text = amount.ToString();
        }
    }
}
