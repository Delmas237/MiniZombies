using Factory;
using UnityEngine;

namespace Weapons
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] private int _magnitude = 100;
        private IInstanceProvider<AudioSource> _destroySoundFactory;

        public void Intialize(IInstanceProvider<AudioSource> destroySoundFactory)
        {
            _destroySoundFactory = destroySoundFactory;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                player.WeaponsController.AddBullets(_magnitude);

                AudioSource audioSource = _destroySoundFactory.GetInstance();
                audioSource.transform.position = transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
