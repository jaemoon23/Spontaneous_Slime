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

        // 레벨업 로드 희귀도 1
        var levelUpTable1 = new LevelUpTable1();
        levelUpTable1.Load(DataTableIds.LevelUp1);
        tables.Add(DataTableIds.LevelUp1, levelUpTable1);

        // 레벨업 로드 희귀도 2
        var levelUpTable2 = new LevelUpTable2();
        levelUpTable2.Load(DataTableIds.LevelUp2);
        tables.Add(DataTableIds.LevelUp2, levelUpTable2);

        // 레벨업 로드 희귀도 3
        var levelUpTable3 = new LevelUpTable3();
        levelUpTable3.Load(DataTableIds.LevelUp3);
        tables.Add(DataTableIds.LevelUp3, levelUpTable3);

        // 레벨업 로드 희귀도 4
        var levelUpTable4 = new LevelUpTable4();
        levelUpTable4.Load(DataTableIds.LevelUp4);
        tables.Add(DataTableIds.LevelUp4, levelUpTable4);

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


    public static LevelUpTable1 LevelUpTable1
    {
        get
        {
            return Get<LevelUpTable1>(DataTableIds.LevelUp1);
        }
    }
    public static LevelUpTable2 LevelUpTable2
    {
        get
        {
            return Get<LevelUpTable2>(DataTableIds.LevelUp2);
        }
    }
    public static LevelUpTable3 LevelUpTable3
    {
        get
        {
            return Get<LevelUpTable3>(DataTableIds.LevelUp3);
        }
    }
    public static LevelUpTable4 LevelUpTable4
    {
        get
        {
            return Get<LevelUpTable4>(DataTableIds.LevelUp4);
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
