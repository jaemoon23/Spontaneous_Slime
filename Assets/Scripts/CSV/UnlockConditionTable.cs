using System.Collections.Generic;
using UnityEngine;

public class UnlockConditionTable : DataTable
{
    private readonly Dictionary<string, UnlockConditionData> table = new Dictionary<string, UnlockConditionData>();
    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<UnlockConditionData>(textAsset.text);

        foreach (var item in list)
        {
            if (!table.ContainsKey(item.UnlockId))
            {
                table.Add(item.UnlockId, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public UnlockConditionData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
