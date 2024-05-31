using ObjectPool;
using UnityEngine;

namespace Weapons
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] private int _magnitude = 100;
        private IPool<AudioSource> _destroySoundPool;

        public void Intialize(IPool<AudioSource> destroySoundPool)
        {
            _destroySoundPool = destroySoundPool;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                player.WeaponsController.AddBullets(_magnitude);

                AudioSource audioSource = _destroySoundPool.GetFreeElement();
                audioSource.transform.position = transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
