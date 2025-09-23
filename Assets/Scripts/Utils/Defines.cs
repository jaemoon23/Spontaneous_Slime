using UnityEngine;

public static class Defines
{
    public static readonly string OnTouch = "OnTouched";

}
public enum EnvironmentType
{
    Light,
    Humidifier,
    AirConditioner,
    Heater,
    PlantPot
}
public static class DataTableIds
{
    public static readonly int[] SlimeIds =
    {
        11011, // 기본 슬라임
        21111, // 빛 슬라임
        31121, // 어둠 슬라임
        41211, // 물 슬라임
        51221, // 얼음 슬라임
        61411, // 불 슬라임
        72031, // 식물 슬라임
        82021, // 고양이 슬라임
        92211, // 비 슬라임
        102311 // 오로라 슬라임   
    };

    public static readonly int[] InteriorIds =
    {
        10001, // 조명
        20001, // 가습기
        30001, // 에어컨
        40001, // 난로
        100001, // 화분
        110001, // 털실
        120001, // 창문
        100002, // 시계
    };

    public static readonly int[] ItemIds =
    {
        1001,   // 에테르
        2101,   // 간식 1
        2102,   // 간식 2
        2103,   // 간식 3
        2104,   // 츄르
        2105,   // 소환석
    };

    public static readonly int[] LevelUpIds1 =
    {
        // 희귀도 1
        10050101,
        10050102,
        10050103,
        10050104,
        10050105,
        10050106,
        10050107,
        10050108,
        10050109,
        10050110,

    };
    public static readonly int[] LevelUpIds2 =
    {
        // 희귀도 2
        10040201,
        10040202,
        10040203,
        10040204,
        10040205,
        10040206,
        10040207,
        10040208,
        10040209,
        10040210,

    };
    public static readonly int[] LevelUpIds3 =
    {
        // 희귀도 3
        10030301,
        10030302,
        10030303,
        10030304,
        10030305,
        10030306,
        10030307,
        10030308,
        10030309,
        10030310,

    };
    public static readonly int[] LevelUpIds4 =
    {
        // 희귀도 4
        10020401,
        10020402,
        10020403,
        10020404,
        10020405,
        10020406,
        10020407,
        10020408,
        10020409,
        10020410,

    };
    public static readonly int[] LevelUpIds5 =
    {
        // 희귀도 5
        10010501,
        10010502,
        10010503,
        10010504,
        10010505,
        10010506,
        10010507,
        10010508,
        10010509,
        10010510,
    };
    
    public static readonly int[] UnlockIds =
    {
        101001, // 기본 슬라임
        111011, // 빛 슬라임
        112011, // 어둠 슬라임
        121021, // 물 슬라임
        122021, // 얼음 슬라임
        122031, // 얼음 슬라임
        141041, // 불 슬라임
        203011, // 식물 슬라임 
        203021, // 식물 슬라임
        203101, // 식물 슬라임
    };

    public static readonly string Slime = "Slime";
    public static readonly string Interior = "INTERIOR";
    public static readonly string Item = "ITEM";
    public static readonly string LevelUp1 = "LEVELUP1";
    public static readonly string LevelUp2 = "LEVELUP2";
    public static readonly string LevelUp3 = "LEVELUP3";
    public static readonly string LevelUp4 = "LEVELUP4";
    public static readonly string LevelUp5 = "LEVELUP5";
    public static readonly string UnlockCondition = "UNLOCK";
    public static readonly string String = "StringTable";
}

public static class Paths
{
    public static readonly string Slime = "Prefabs/Slime";
    public static readonly string ScriptWindow = "Prefabs/ScriptWindow";
    public static readonly string SlimeSpawnTextWindow = "Prefabs/SlimeSpawnTextWindow";
    public static readonly string SlimeInfo = "Prefabs/SlimeInfo";
    public static readonly string MailButton = "Prefabs/MailButton";
    public static readonly string Mail = "Prefabs/Mail";
    public static readonly string ShopSlot = "Prefabs/ShopSlot";
    public static readonly string InvenSlot = "Prefabs/InvenSlot";
}

public static class Tags
{
    public static readonly string Player = "Player";
    public static readonly string PlayerExpression = "PlayerExpression";
    public static readonly string SlimeManager = "SlimeManager";
    public static readonly string UiManager = "UiManager";
    public static readonly string GameManager = "GameController";
    public static readonly string EnvironmentManager = "EnvironmentManager";
    public static readonly string CollectionManager = "CollectionManager";
    public static readonly string SlimeInfoPanel = "infoPanel";
    public static readonly string CollectionPanel = "CollectionPanel";
    public static readonly string MailPanel = "MailPanel";

    public static readonly string WarningPanel = "WarningPanel";
}

public static class ObjectNames
{
    public static readonly string SlimeBody = "SlimeBody";
    public static readonly string Sphere = "구체";
}

public static class Strings
{
    public static readonly string Gold = "Gold";
}