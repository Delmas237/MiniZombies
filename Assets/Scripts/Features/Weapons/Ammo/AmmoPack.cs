using UnityEngine;

namespace Weapons
{
    public class AmmoPack : MonoBehaviour
    {
        [SerializeField] private int _magnitude = 100;
        public IInstanceProvider<AudioSource> DestroySoundFactory { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                player.WeaponsModule.AddBullets(_magnitude);

                AudioSource audioSource = DestroySoundFactory.GetInstance();
                audioSource.transform.position = transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
