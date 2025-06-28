using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Weapons
{
    public static class GunsDataSaver
    {
        private static bool _initialized;
        private static string _dataKey = "Guns";

        private static IDataSaver<GunSavableData> _dataSaver;

        private static readonly Dictionary<GunType, GunData> _gunsData = new Dictionary<GunType, GunData>();
        private static readonly Dictionary<GunType, GunSavableData> _gunsSavableData = new Dictionary<GunType, GunSavableData>();

        public static event Action Initialized;

        public static IReadOnlyDictionary<GunType, GunData> GunsData => _gunsData;
        public static IReadOnlyDictionary<GunType, GunSavableData> GunsSavableData
        {
            get
            {
                if (!_initialized)
                    Initialize();
                return _gunsSavableData;
            }
        }

        public static void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;
            CoroutineHelper.StartRoutine(InitializeCor());
        }
        private static IEnumerator InitializeCor()
        {
            AsyncOperationHandle<IList<GunData>> handle = Addressables.LoadAssetsAsync<GunData>(_dataKey, null);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<GunData> gunsData = handle.Result;
                foreach (var gunData in gunsData)
                    _gunsData.Add(gunData.Type, gunData);
            }
            else
            {
                Debug.LogError("Failed to load data");
            }
            _dataSaver = new JsonGunsDataSaver();
            Load();

            Initialized?.Invoke();
        }

        private static void Load()
        {
            if (!_initialized)
                Initialize();

            List<GunSavableData> data = _dataSaver.Load();

            foreach (var gunData in data)
                _gunsSavableData.Add(gunData.Type, gunData);
        }

        public static void Save(GunSavableData data)
        {
            if (!_initialized)
                Initialize();

            _dataSaver.Save(data);
        }

        public static bool InitializeGun(Gun gun)
        {
            if (!_initialized)
                Initialize();

            if (_gunsSavableData.ContainsKey(gun.Type))
            {
                GunSavableData gunSavableData = _gunsSavableData[gun.Type];

                gun.Damage = gunSavableData.Damage;
                gun.Cooldown = gunSavableData.Cooldown;
                gun.Distance = gunSavableData.Distance;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
