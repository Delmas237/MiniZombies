using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Weapons
{
    public static class GunsDataSaver
    {
        private static bool _initialized;
        private const string RESOURCES_DATA_PATH = "Data/Guns";

        private static readonly Dictionary<GunType, GunData> _gunsData = new Dictionary<GunType, GunData>();
        public static IReadOnlyDictionary<GunType, GunData> GunsData => _gunsData;

        private static readonly Dictionary<GunType, GunSaveableData> _gunsSaveableData = new Dictionary<GunType, GunSaveableData>();
        public static IReadOnlyDictionary<GunType, GunSaveableData> GunsSaveableData
        {
            get
            {
                if (!_initialized)
                    Initialize();
                return _gunsSaveableData;
            }
        }

        private static void Initialize()
        {
            _initialized = true;
            GunData[] gunsData = Resources.LoadAll<GunData>(RESOURCES_DATA_PATH);

            foreach (GunData gunData in gunsData)
                _gunsData.Add(gunData.Type, gunData);

            int allGunTypes = Enum.GetValues(typeof(GunType)).Length;

            for (int i = 0; i < allGunTypes; i++)
            {
                GunType currentType = (GunType)i;
                string path = GetPathForGun(currentType);

                if (!File.Exists(path))
                {
                    GunSaveableData gunSaveableData = new GunSaveableData(_gunsData[currentType]);
                    SaveData(gunSaveableData);
                }

                string serializedGun = File.ReadAllText(path);
                GunSaveableData gunData = JsonUtility.FromJson<GunSaveableData>(serializedGun);
                _gunsSaveableData.Add(gunData.Type, gunData);
            }
        }

        public static bool LoadData(Gun gun)
        {
            if (!_initialized)
                Initialize();

            if (_gunsSaveableData.ContainsKey(gun.Type))
            {
                GunSaveableData gunSaveableData = _gunsSaveableData[gun.Type];

                gun.Damage = gunSaveableData.Damage;
                gun.Cooldown = gunSaveableData.Cooldown;
                gun.Distance = gunSaveableData.Distance;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SaveData(GunSaveableData gunSaveableData)
        {
            if (!_initialized)
                Initialize();

            string serializedGun = JsonUtility.ToJson(gunSaveableData);

            string path = GetPathForGun(gunSaveableData.Type);
            ValidateDirectory(path);

            File.WriteAllText(path, serializedGun);

#if UNITY_EDITOR
            Debug.Log($"Gun json saved: {serializedGun}");
#endif
        }

        private static void ValidateDirectory(string path)
        {
            string directoryPath = Path.GetDirectoryName(path);
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        private static string GetPathForGun(GunType gunType)
        {
            string dataPath = Application.isMobilePlatform ? Application.persistentDataPath : Application.dataPath;
            return $"{dataPath}/JsonData/{gunType}.json";
        }
    }
}
