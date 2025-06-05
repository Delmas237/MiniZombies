using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShop : MonoBehaviour
    {
        [SerializeField] private List<GlobalShopItem> _items;

        private void Awake()
        {
            foreach (var item in _items)
            {
                item.Intialize();
                item.Updated += GunsDataSaver.SaveData;
            }
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
                Bank.Add(100);
        }

        private void OnDestroy()
        {
            foreach (var item in _items)
            {
                item.Updated -= GunsDataSaver.SaveData;
                item.OnDestroy();
            }
        }
    }
}
