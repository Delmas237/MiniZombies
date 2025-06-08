using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Weapons;

public class JsonGunsDataSaver : IDataSaver<GunSavableData>
{
    public List<GunSavableData> Load()
    {
        int allGunTypes = Enum.GetValues(typeof(GunType)).Length;

        List<GunSavableData> allData = new List<GunSavableData>();
        for (int i = 0; i < allGunTypes; i++)
        {
            GunType currentType = (GunType)i;
            string path = GetDataPath(currentType);

            if (!File.Exists(path))
            {
                GunSavableData gunSavableData = new GunSavableData(GunsDataSaver.GunsData[currentType]);
                Save(gunSavableData);
            }

            string serializedGun = File.ReadAllText(path);
            GunSavableData gunData = JsonUtility.FromJson<GunSavableData>(serializedGun);
            allData.Add(gunData);
        }

        return allData;
    }

    public void Save(GunSavableData data)
    {
        string serializedGun = JsonUtility.ToJson(data);

        string path = GetDataPath(data.Type);
        ValidateDirectory(path);

        File.WriteAllText(path, serializedGun);

#if UNITY_EDITOR
        Debug.Log($"Gun json saved: {serializedGun}");
#endif
    }

    private void ValidateDirectory(string path)
    {
        string directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }

    private string GetDataPath(GunType gunType)
    {
        string dataPath = Application.isMobilePlatform ? Application.persistentDataPath : Application.dataPath;
        return $"{dataPath}/JsonData/{gunType}.json";
    }
}
