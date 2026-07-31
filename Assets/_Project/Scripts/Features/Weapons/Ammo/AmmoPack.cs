using EntityLib;
using UnityEngine;

namespace Weapons
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] private int _magnitude = 100;
        public IInstanceProvider<AudioSource> DestroySoundFactory { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEntity entity) && entity.RoleModule.Role == EntityRole.Player)
            {
                var weaponModule = entity.GetModule<IEntityWeaponModule>();
                weaponModule.AddBullets(_magnitude);

                AudioSource audioSource = DestroySoundFactory.GetInstance();
                audioSource.transform.position = transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
