using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public static class GunsDataSaver
    {
        private static List<GunData> _gunsData = new List<GunData>();
        public static IReadOnlyList<GunData> GunsData => _gunsData;

        public static void Load(List<GunData> guns)
        {
            _gunsData = guns;

            int allGunTypes = Enum.GetValues(typeof(GunType)).Length;

            List<GunJSONData> gunsJSONData = new List<GunJSONData>();
            for (int i = 0; i < allGunTypes; i++)
            {
                string path = GetPathForGun((GunType)i);

                if (!File.Exists(path))
                    Save();

                string serializedGun = File.ReadAllText(path);
                GunJSONData gunData = JsonUtility.FromJson<GunJSONData>(serializedGun);
                gunsJSONData.Add(gunData);
            }
            CopyJSONToGunsData(gunsJSONData);
        }

        public static void CopyToGunsData(List<Gun> guns)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (GunsData.Any(g => g.Type == guns[i].Type))
                {
                    Gun gun = guns[i];
                    GunData gunData = GunsData.First(g => g.Type == gun.Type);

                    gun.Damage = gunData.Damage;
                    gun.Cooldown = gunData.Cooldown;
                    gun.Distance = gunData.Distance;
                }
            }
        }
        private static void CopyJSONToGunsData(List<GunJSONData> gunsJSONData)
        {
            for (int i = 0; i < _gunsData.Count; i++)
            {
                if (gunsJSONData.Any(g => g.Type == _gunsData[i].Type))
                {
                    GunData gunData = _gunsData[i];
                    GunJSONData gunJSONData = gunsJSONData.First(g => g.Type == _gunsData[i].Type);

                    gunData.Damage = gunJSONData.Damage;
                    gunData.Cooldown = gunJSONData.Cooldown;
                    gunData.Distance = gunJSONData.Distance;
                }
            }
        }

        public static void Save()
        {
            List<GunJSONData> gunsJSONData = new List<GunJSONData>();
            foreach (GunData gunData in _gunsData)
            {
                GunJSONData gunJSONData = new GunJSONData(gunData.Type, gunData.Damage, gunData.Cooldown, gunData.Distance);
                gunsJSONData.Add(gunJSONData);

                string serialzedGun = JsonUtility.ToJson(gunJSONData);

                string path = GetPathForGun(gunJSONData.Type);
                ValidateDirectory(path);

                File.WriteAllText(path, serialzedGun);

#if UNITY_EDITOR
                Debug.Log($"Gun json saved: {serialzedGun}");
#endif
            }
        }

        private static void ValidateDirectory(string path)
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        private static string GetPathForGun(GunType gunType) => $"{Application.dataPath}/JsonData/{gunType}.json";
    }
}
