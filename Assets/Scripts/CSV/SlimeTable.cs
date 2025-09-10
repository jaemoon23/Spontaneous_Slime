using System.Collections.Generic;
using UnityEngine;

public class SlimeTable : DataTable
{
    private readonly Dictionary<string, SlimeData> table = new Dictionary<string, SlimeData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SlimeData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.SlimeId))
            {
                table.Add(item.SlimeId, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }

    }

    public SlimeData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
