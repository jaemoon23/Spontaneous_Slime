using System.Collections.Generic;
using UnityEngine;

public class LevelUpTable3 : DataTable
{
    private readonly Dictionary<int, LevelUpData3> table = new Dictionary<int, LevelUpData3>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<LevelUpData3>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.LevelId))
            {
                table.Add(item.LevelId, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }

    }

    public LevelUpData3 Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}