using PlayerLib;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    public class CooldownCircle : MonoBehaviour
    {
        [SerializeField] private PlayerContainer _player;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            UpdateCircle();
        }

        private void UpdateCircle()
        {
            Gun gun = _player.WeaponsController.CurrentGun;
            _image.fillAmount = gun.CurrentCooldown / gun.Cooldown;
        }
    }
}
