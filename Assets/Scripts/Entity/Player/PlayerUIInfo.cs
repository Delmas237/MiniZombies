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

        private void Update()
        {
            if (_player != null)
            {
                _healthBar.fillAmount = _player.HealthController.Health / _player.HealthController.MaxHealth;
                _bullets.text = _player.WeaponsController.Bullets.ToString();
                _coins.text = _player.CurrencyController.Coins.ToString();
            }
        }
    }
}
