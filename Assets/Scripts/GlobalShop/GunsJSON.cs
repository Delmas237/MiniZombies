using System.IO;
using UnityEngine;

namespace Weapons
{
    public class GunsJSON
    {
        private static string PathToFile;

        public float[] Damage;
        public float[] Cooldown;
        public float[] Distance;

        public GunsJSON(GunData[] guns)
        {
            Damage = new float[guns.Length];
            Cooldown = new float[guns.Length];
            Distance = new float[guns.Length];

            for (int i = 0; i < guns.Length; i++)
            {
                Damage[i] = guns[i].Damage;
                Cooldown[i] = guns[i].Cooldown;
                Distance[i] = guns[i].Distance;
            }
        }

        public static string GetPath()
        {
            if (Application.isMobilePlatform)
                PathToFile = Application.persistentDataPath + "/SavesJSON/Guns.json";
            else
                PathToFile = Application.dataPath + "/SavesJSON/Guns.json";

            string directoryPath = Path.GetDirectoryName(PathToFile);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return PathToFile;
        }
    }
}
