using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public static class GunsDataSaver
    {
        private static bool _initialized;
        private const string RESOURCES_DATA_PATH = "Data/Guns";

        private static IDataSaver<GunSavableData> _dataSaver;

        private static readonly Dictionary<GunType, GunData> _gunsData = new Dictionary<GunType, GunData>();
        private static readonly Dictionary<GunType, GunSavableData> _gunsSavableData = new Dictionary<GunType, GunSavableData>();

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

        private static void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;
            GunData[] gunsData = Resources.LoadAll<GunData>(RESOURCES_DATA_PATH);

            foreach (GunData gunData in gunsData)
                _gunsData.Add(gunData.Type, gunData);

            _dataSaver = new JsonGunsDataSaver();
            Load();
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
