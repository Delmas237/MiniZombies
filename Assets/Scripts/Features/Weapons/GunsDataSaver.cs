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

        private static readonly Dictionary<GunType, GunSavableData> _gunsSavableData = new Dictionary<GunType, GunSavableData>();
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
                    GunSavableData gunSaveableData = new GunSavableData(_gunsData[currentType]);
                    SaveData(gunSaveableData);
                }

                string serializedGun = File.ReadAllText(path);
                GunSavableData gunData = JsonUtility.FromJson<GunSavableData>(serializedGun);
                _gunsSavableData.Add(gunData.Type, gunData);
            }
        }

        public static bool LoadData(Gun gun)
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

        public static void SaveData(GunSavableData gunSavableData)
        {
            if (!_initialized)
                Initialize();

            string serializedGun = JsonUtility.ToJson(gunSavableData);

            string path = GetPathForGun(gunSavableData.Type);
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
