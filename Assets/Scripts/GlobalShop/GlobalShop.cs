using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShop : MonoBehaviour
    {
        [SerializeField] private GunsData gunsData;
        [SerializeField] private GameObject localWeapons;
        [SerializeField] private GlobalShopItem[] items;

        private void Awake()
        {
            GunsManager.Load(gunsData.Guns);
            Gun[] guns = GunsManager.GameObjectToGuns(localWeapons);
            GunsManager.CopyGunsValuesTo(ref guns);

            for (int i = 0; i < items.Length; i++)
                items[i].Updated += GunsManager.Save;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Cheats();
#endif
        }

        private void Cheats()
        {
            if (Input.GetKeyDown(KeyCode.N))
                Bank.Coins += 100;
            if (Input.GetKeyDown(KeyCode.B))
                GunsManager.Save();
        }
    }
}
