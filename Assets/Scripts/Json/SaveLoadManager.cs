using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using SaveDataVC = SaveDataV1;  // 현재 버전에 맞춰 변경
public class SaveLoadManager
{
   public SaveLoadManager()
    {
        SaveLoadManager.Load();
    }
    // 현재 버전에 맞춰 변경
    public static int SaveDataVersion { get; } = 1;

    public static SaveDataVC Data { get; set; } = new SaveDataVC();
    private static readonly string[] SaveFileName =
    {
        "SaveAuto.json",
    };

    public static string SaveDirectory => $"{Application.persistentDataPath}/Save";
    public static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        Converters = { new Vector3Converter() }
    };

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
        {
            return false;
        }
        try
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            // 직렬화
            var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
            var json = JsonConvert.SerializeObject(Data, settings);

            // 암호화


            File.WriteAllText(path, json);
            return true;
        }
        catch
        {
            Debug.LogError("Save 예외 발생");
            return false;
        }
    }
    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
        {
            return false;
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            var json = File.ReadAllText(path);
            var dataSave = JsonConvert.DeserializeObject<SaveData>(json, settings);

            while (dataSave.Version < SaveDataVersion)
            {
                dataSave = dataSave.VersionUpgrade();
            }

            Data = dataSave as SaveDataVC;
            return true;
        }
        catch
        {
            Debug.LogError("Load 예외 발생");
            return false;
        }
       
    }
}
