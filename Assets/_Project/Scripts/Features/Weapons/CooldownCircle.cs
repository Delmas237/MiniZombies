using Entity.Friendly.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    public class CooldownCircle : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _player;
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
            Gun gun = _player.WeaponModule.CurrentGun;
            _image.fillAmount = gun.CurrentCooldown / gun.Cooldown;
        }
    }
}
