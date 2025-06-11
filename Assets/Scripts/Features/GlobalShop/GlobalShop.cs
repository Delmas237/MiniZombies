using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShop : MonoBehaviour
    {
        [SerializeField] private List<GlobalShopItem> _items;
        [SerializeField] private string _dataPath = "Data/GlobalShopData";
        private GlobalShopData _data;

        private void Awake()
        {
            _data = Resources.Load<GlobalShopData>(_dataPath);
            
            foreach (var item in _items)
            {
                item.Intialize(_data.Items.First(i => i.Type == item.Type));
                item.Updated += GunsDataSaver.Save;
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
                item.Updated -= GunsDataSaver.Save;
            }
        }
    }
}
