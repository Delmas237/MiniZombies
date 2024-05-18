using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Weapons
{
    public static class GunsManager
    {
        private static GunData[] _guns;
        public static IReadOnlyList<GunData> Guns => _guns;

        public static void Load(GunData[] guns)
        {
            _guns = guns;
            LoadValuesFromJSON();
        }

        private static void LoadValuesFromJSON()
        {
            if (File.Exists(GunsJSON.GetPath()))
            {
                GunsJSON gunsJSON = JsonUtility.FromJson<GunsJSON>(File.ReadAllText(GunsJSON.GetPath()));
                CopyGunsValues(gunsJSON);
            }
            else //First save
            {
                Save();
            }
        }

        public static void CopyGunsValuesTo(ref Gun[] guns)
        {
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].Damage = Guns[i].Damage;
                guns[i].Cooldown = Guns[i].Cooldown;
                guns[i].Distance = Guns[i].Distance;
            }
        }
        private static void CopyGunsValues(GunsJSON gunsJSON)
        {
            for (int i = 0; i < _guns.Length; i++)
            {
                _guns[i].Damage = gunsJSON.Damage[i];
                _guns[i].Cooldown = gunsJSON.Cooldown[i];
                _guns[i].Distance = gunsJSON.Distance[i];
            }
        }

        public static void Save()
        {
            GunsJSON gunsJSON = new GunsJSON(_guns);
            File.WriteAllText(GunsJSON.GetPath(), JsonUtility.ToJson(gunsJSON));

            Debug.Log($"Guns json saved: {JsonUtility.ToJson(gunsJSON)}");
        }

        public static Gun[] GameObjectToGuns(GameObject gunsPrefab) => gunsPrefab.GetComponentsInChildren<Gun>(true);
    }
}
