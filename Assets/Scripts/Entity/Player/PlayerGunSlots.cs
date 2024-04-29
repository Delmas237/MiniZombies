using UnityEngine;
using Weapons;

namespace PlayerLib
{
    public class PlayerGunSlots : MonoBehaviour
    {
        [SerializeField] private PlayerContainer player;

        [SerializeField] private GameObject[] gunsSlot2;

        public GunType GunSlot1 { get; private set; } = GunType.Pistol;
        public GunType GunSlot2 { get; set; }

        [SerializeField] private AudioSource getGunSound;

        private void Update()
        {
            for (int i = 0; i < gunsSlot2.Length; i++)
            {
                if (i == (int)GunSlot2 - 1)
                    gunsSlot2[i].SetActive(true);
                else
                    gunsSlot2[i].SetActive(false);
            }
        }

        public void ChangeGunSlot(int slot)
        {
            if (slot == 1 && player.WeaponsController.CurrentGun.Type != GunSlot1)
            {
                player.WeaponsController.ChangeGun(GunSlot1);
                getGunSound.Play();
            }
            if (slot == 2 && player.WeaponsController.CurrentGun.Type != GunSlot2)
            {
                player.WeaponsController.ChangeGun(GunSlot2);
                getGunSound.Play();
            }
        }
    }
}
