using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalShopLib
{
    [CreateAssetMenu(fileName = "LocalShopData", menuName = "LocalShopData")]
    public class LocalShopData : ScriptableObject
    {
        [SerializeField] private List<LocalShopItemData> _items;
        [SerializeField] private List<LocalShopGunData> _weapons;

        public List<LocalShopItemData> Items => _items;
        public List<LocalShopGunData> Weapons => _weapons;
    }
}
