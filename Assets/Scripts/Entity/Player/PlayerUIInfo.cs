using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerLib
{
    public class PlayerUIInfo : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI bullets;
        [SerializeField] private TextMeshProUGUI coins;

        private void Update()
        {
            if (player)
            {
                healthBar.fillAmount = player.HealthController.Health * 0.01f;
                bullets.text = player.WeaponsController.Bullets.ToString();
                coins.text = player.Coins.ToString();
            }
        }
    }
}
