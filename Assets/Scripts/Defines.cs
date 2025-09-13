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
    public static readonly string[] SlimeIds =
    {
        "11011", // 기본 슬라임
        "21111", // 빛 슬라임
        "31121", // 어둠 슬라임
        "41211", // 물 슬라임
        "51221", // 얼음 슬라임
        "61411", // 불 슬라임
        "72031", // 식물 슬라임
    };

    public static readonly string[] ItemIds =
    {
        "10001",  // 조명
        "20001",  // 가습기
        "30001",  // 에어컨
        "40001",  // 난로
        "100001", // 화분
    };

    public static readonly string[] LevelUpIds =
    {
        "10010101", // level 1
        "10010201", // level 2
        "10010311", // level 3
        "10010401", // level 4
        "10010501", // level 5
        "10010611", // level 6
        "10010701", // level 7
        "10010801", // level 8
        "10010911", // level 9
        "10011021", // level 10
    };

    public static readonly string[] UnlockConditionIds =
    {
        "101001", // 해금 조건 그룹 101
        "111011", // 해금 조건 그룹 111
        "112011", // 해금 조건 그룹 112
        "121021", // 해금 조건 그룹 121
        "122021", // 해금 조건 그룹 122
        "122031", // 해금 조건 그룹 122
        "141041", // 해금 조건 그룹 141
        "203011", // 해금 조건 그룹 203
        "203021", // 해금 조건 그룹 203
        "203101"  // 해금 조건 그룹 203
    };

    public static readonly string Slime = "Slime";
    public static readonly string Item = "Item";
    public static readonly string LevelUp = "LevelUP";
    public static readonly string UnlockCondition = "Unlock_conditions";
    public static readonly string String = "StringTable";
}

public static class Paths
{
    public static readonly string Slime = "Prefabs/Slime";
    public static readonly string ScriptWindow = "Prefabs/ScriptWindow";
}

public static class Tags
{
    public static readonly string SlimeManager = "SlimeManager";
    public static readonly string UiManager = "UiManager";
    public static readonly string GameManager = "GameController";
    public static readonly string EnvironmentManager = "EnvironmentManager";
}