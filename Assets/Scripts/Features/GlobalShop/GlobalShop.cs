using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShop : MonoBehaviour
    {
        [SerializeField] private List<GlobalShopItem> _items;
        [SerializeField] private string _dataKey = "GlobalShopData";
        private GlobalShopData _data;

        private void Awake()
        {
            GunsDataSaver.Initialize();
            GunsDataSaver.Initialized += Initialize;
        }

        private void Initialize() => StartCoroutine(InitializeCor());
        private IEnumerator InitializeCor()
        {
            yield return StartCoroutine(LoadData());

            foreach (var item in _items)
            {
                item.Intialize(_data.Items.First(i => i.Type == item.Type));
                item.Updated += GunsDataSaver.Save;
            }
        }
        private IEnumerator LoadData()
        {
            AsyncOperationHandle<GlobalShopData> handle = Addressables.LoadAssetAsync<GlobalShopData>(_dataKey);

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                _data = handle.Result;
            else
                Debug.LogError("Failed to load data");
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
            GunsDataSaver.Initialized -= Initialize;

            foreach (var item in _items)
                item.Updated -= GunsDataSaver.Save;
        }
    }
}
