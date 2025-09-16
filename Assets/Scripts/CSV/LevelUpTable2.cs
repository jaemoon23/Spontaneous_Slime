using System.Collections.Generic;
using UnityEngine;

public class LevelUpTable2 : DataTable
{
    private readonly Dictionary<int, LevelUpData2> table = new Dictionary<int, LevelUpData2>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<LevelUpData2>(textAsset.text);

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

    public LevelUpData2 Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
