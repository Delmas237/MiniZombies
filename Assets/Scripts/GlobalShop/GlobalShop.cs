using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShop : MonoBehaviour
    {
        [SerializeField] private GunsData _gunsData;
        [SerializeField] private List<Gun> _guns;
        [SerializeField] private List<GlobalShopItem> _items;

        private void Awake()
        {
            GunsDataSaver.Load(_gunsData.Guns);
            GunsDataSaver.CopyToGunsData(_guns);

            foreach (var item in _items)
            {
                item.Intialize();
                item.Updated += GunsDataSaver.Save;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in _items)
                item.OnDestroy();
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
                GunsDataSaver.Save();
        }
    }
}
