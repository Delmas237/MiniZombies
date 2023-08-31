using PlayerLib;
using UnityEngine;
using ObjectPool;

namespace Weapons
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] private int magnitude;
        public IPool<AudioSource> DestroySoundPool { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.WeaponsController.Bullets += magnitude;

                AudioSource audioSource = DestroySoundPool.GetFreeElement();
                audioSource.transform.position = transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
