using System.Collections.Generic;
using UnityEngine;

public class InteriorTable : DataTable
{
    private readonly Dictionary<int, InteriorData> table = new Dictionary<int, InteriorData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<InteriorData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.InteriorId))
            {
                table.Add(item.InteriorId, item);
            }
            else
            {
                Debug.LogError("인테리어 아이디 중복!");
            }
        }

    }

    public InteriorData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
