using PlayerLib;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    public class CooldownCircle : MonoBehaviour
    {
        [SerializeField] private PlayerContainer player;
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            UpdateCircle();
        }

        private void UpdateCircle()
        {
            Gun gun = player.WeaponsController.CurrentGun;
            image.fillAmount = gun.CurrentCooldown / gun.Cooldown;
        }
    }
}
