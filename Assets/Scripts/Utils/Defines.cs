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

    public static readonly string[] ItemIds =
    {
        "10001",  // 조명
        "20001",  // 가습기
        "30001",  // 에어컨
        "40001",  // 난로
        "100001", // 화분
    };

    public static readonly int[] LevelUpIds1 =
    {
        // 희귀도 1
        10040101, // 1레벨
        10040102, // 2레벨
        10040103, // 3레벨
        10040104, // 4레벨
        10040105, // 5레벨
        10040106, // 6레벨
        10040107, // 7레벨
        10040108, // 8레벨
        10040109, // 9레벨
        10040110, // 10레벨
    };
    public static readonly int[] LevelUpIds2 =
    {
        // 희귀도 2
        10030201,   // 1레벨
        10030202,   // 2레벨
        10030203,   // 3레벨
        10030204,   // 4레벨
        10030205,   // 5레벨
        10030206,   // 6레벨
        10030207,   // 7레벨
        10030208,   // 8레벨
        10030209,   // 9레벨
        10030210,   // 10레벨
    };
    public static readonly int[] LevelUpIds3 =
    {
        // 희귀도 3
        10020301,   // 1레벨
        10020302,   // 2레벨
        10020303,   // 3레벨
        10020304,   // 4레벨    
        10020305,   // 5레벨
        10020306,   // 6레벨
        10020307,   // 7레벨
        10020308,   // 8레벨
        10020309,   // 9레벨
        10020310,   // 10레벨
    };
    public static readonly int[] LevelUpIds4 =
    {
        // 희귀도 4
        10010401,   // 1레벨
        10010402,   // 2레벨
        10010403,   // 3레벨
        10010404,   // 4레벨
        10010405,   // 5레벨
        10010406,   // 6레벨
        10010407,   // 7레벨
        10010408,   // 8레벨
        10010409,   // 9레벨
        10010410,   // 10레벨
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