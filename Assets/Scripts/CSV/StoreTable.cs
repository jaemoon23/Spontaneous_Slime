using System.Collections.Generic;
using UnityEngine;

public class StoreTable : DataTable
{
    private readonly Dictionary<int, StoreData> table = new Dictionary<int, StoreData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StoreData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.productId))
            {
                table.Add(item.productId, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public StoreData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
