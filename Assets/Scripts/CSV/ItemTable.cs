using System.Collections.Generic;
using UnityEngine;

public class ItemTable : DataTable
{
    private readonly Dictionary<int, ItemData> table = new Dictionary<int, ItemData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ItemData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.ItemId))
            {
                table.Add(item.ItemId, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }

    }

    public ItemData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
