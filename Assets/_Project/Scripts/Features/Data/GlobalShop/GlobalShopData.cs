using System.Collections.Generic;
using UnityEngine;

namespace GlobalShopLib
{
    [CreateAssetMenu(fileName = "GlobalShopData", menuName = "GlobalShopData")]
    public class GlobalShopData : ScriptableObject
    {
        [SerializeField] private List<GlobalShopItemData> _items;

        public List<GlobalShopItemData> Items => _items;
    }
}
