using UnityEngine;

public class JsonTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoadManager.Data = new SaveDataV1();
            SaveLoadManager.Data.SlimeName = "TEST";
            SaveLoadManager.Save();
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.Load();

            Debug.Log(SaveLoadManager.Data.SlimeName);
        }
    }
}
