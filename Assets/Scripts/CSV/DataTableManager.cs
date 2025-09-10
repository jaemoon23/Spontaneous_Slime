using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
        // 슬라임 로드
        var slimeTable = new SlimeTable();
        slimeTable.Load(DataTableIds.Slime);
        tables.Add(DataTableIds.Slime, slimeTable);

        // 아이템 로드
        var itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

        // 레벨업 로드
        var levelUpTable = new LevelUpTable();
        levelUpTable.Load(DataTableIds.LevelUp);
        tables.Add(DataTableIds.LevelUp, levelUpTable);

        // 언락조건 로드
        var unlockConditionTable = new UnlockConditionTable();
        unlockConditionTable.Load(DataTableIds.UnlockCondition);
        tables.Add(DataTableIds.UnlockCondition, unlockConditionTable);

        // 스트링 테이블 로드
        var stringTable = new StringTable();
        stringTable.Load(DataTableIds.String);
        tables.Add(DataTableIds.String, stringTable);
    }

    public static SlimeTable SlimeTable
    {
        get
        {
            return Get<SlimeTable>(DataTableIds.Slime);
        }
    }

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(DataTableIds.Item);
        }
    }
    public static LevelUpTable LevelUpTable
    {
        get
        {
            return Get<LevelUpTable>(DataTableIds.LevelUp);
        }
    }
    public static UnlockConditionTable UnlockConditionTable
    {
        get
        {
            return Get<UnlockConditionTable>(DataTableIds.UnlockCondition);
        }
    }
    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String);
        }
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}
