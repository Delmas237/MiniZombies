using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Weapons
{
    public static class GunsManager
    {
        private static GunData[] guns;
        public static IReadOnlyList<GunData> Guns => guns;

        public static void Load(GunData[] guns)
        {
            GunsManager.guns = guns;
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
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].Damage = gunsJSON.Damage[i];
                guns[i].Cooldown = gunsJSON.Cooldown[i];
                guns[i].Distance = gunsJSON.Distance[i];
            }
        }

        public static void Save()
        {
            GunsJSON gunsJSON = new GunsJSON(guns);
            File.WriteAllText(GunsJSON.GetPath(), JsonUtility.ToJson(gunsJSON));

            Debug.Log($"Guns json saved: {JsonUtility.ToJson(gunsJSON)}");
        }

        public static Gun[] GameObjectToGuns(GameObject gunsPrefab) => gunsPrefab.GetComponentsInChildren<Gun>(true);
    }
}
