using EntityLib;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    public class CooldownCircle : MonoBehaviour
    {
        [SerializeField] private Entity _player;
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
            var weaponModule = _player.GetModule<IEntityWeaponModule>();
            
            Gun gun = weaponModule.CurrentGun;
            _image.fillAmount = gun.CurrentCooldown / gun.Cooldown;
        }
    }
}
