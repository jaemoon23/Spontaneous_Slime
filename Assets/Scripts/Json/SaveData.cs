using System;
using UnityEngine;

[Serializable]
public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUpgrade();
}

[Serializable]
public class SaveDataV1 : SaveData
{
    public string SlimeName { get; set; }
    
    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUpgrade()
    {
        // 업그레이드 하면 여기에 구현
        return this;
    }
}
