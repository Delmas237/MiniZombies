using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EntityLib.Friendly.Player
{
    public class PlayerUIInfo : MonoBehaviour
    {
        [SerializeField] private Entity _player;
        [Space(10)]
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _bullets;
        [SerializeField] private TextMeshProUGUI _coins;

        private void Start()
        {
            if (_player == null)
                return;
            
            var healthModule = _player.HealthModule;
            var currencyModule = _player.GetModule<IPlayerCurrencyModule>();
            var weaponModule = _player.GetModule<IEntityWeaponModule>();

            UpdateHealthBar();
            UpdateCoinsText(currencyModule.Coins);
            UpdateBulletsText(weaponModule.Bullets);

            healthModule.Increased += UpdateHealthBar;
            healthModule.Decreased += UpdateHealthBar;

            currencyModule.CoinsChanged += UpdateCoinsText;
            weaponModule.BulletsChanged += UpdateBulletsText;
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
            if (_player == null)
                return;

            var healthModule = _player.HealthModule;
            var currencyModule = _player.GetModule<IPlayerCurrencyModule>();
            var weaponModule = _player.GetModule<IEntityWeaponModule>();

            healthModule.Increased -= UpdateHealthBar;
            healthModule.Decreased -= UpdateHealthBar;

            currencyModule.CoinsChanged -= UpdateCoinsText;
            weaponModule.BulletsChanged -= UpdateBulletsText;
        }
    }
}
