using System;
using System.IO;
using System.Text;
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
        
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        
        try
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            // 직렬화
            var json = JsonConvert.SerializeObject(Data, settings);

            // 암호화


            // UTF-8 인코딩으로 안드로이드 호환성 보장
            File.WriteAllText(path, json, Encoding.UTF8);
            return true;
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.LogError($"저장 권한 없음 (안드로이드): {e.Message}");
            return false;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogError($"디렉토리 접근 실패: {e.Message}");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Save 예외 발생: {e.Message}");
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
            // UTF-8 인코딩으로 안드로이드 호환성 보장
            var json = File.ReadAllText(path, Encoding.UTF8);
            var dataSave = JsonConvert.DeserializeObject<SaveData>(json, settings);

            while (dataSave.Version < SaveDataVersion)
            {
                dataSave = dataSave.VersionUpgrade();
            }

            Data = dataSave as SaveDataVC;
            return true;
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError($"저장 파일을 찾을 수 없음: {e.Message}");
            return false;
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.LogError($"파일 접근 권한 없음 (안드로이드): {e.Message}");
            return false;
        }
        catch (JsonException e)
        {
            Debug.LogError($"JSON 파싱 실패: {e.Message}");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load 예외 발생: {e.Message}");
            return false;
        }
       
    }
}
