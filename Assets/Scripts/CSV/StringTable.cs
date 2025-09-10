using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    private readonly Dictionary<string, StringData> table = new Dictionary<string, StringData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StringData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.key))
            {
                table.Add(item.key, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public StringData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
