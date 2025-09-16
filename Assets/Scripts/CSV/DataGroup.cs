using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;

// 레벨업 데이터 공통 인터페이스
public interface ILevelUpData
{
    int LevelId { get; set; }
    int SlimeRarity { get; set; }
    int CurrentLevel { get; set; }
    int NeedExp { get; set; }
    int ScaleLevel { get; set; }
    int EventType { get; set; }
}

#region SlimeData
[Serializable]
public class SlimeData
{
    [Name("SLIME_ID")] public int SlimeId { get; set; }
    [Name("SLIME_NAME")] public string SlimeNameId { get; set; }
    [Name("SLIME_TYPE")] public int SlimeTypeId { get; set; }
    [Name("RARITY")] public int RarityId { get; set; }
    [Name("GIFT_ITEM_ID")] public string GiftItemId { get; set; }
    [Name("SLIME_SCRIPT")] public string SlimeScriptId { get; set; }
    [Name("SLIME_INFORMATION")] public string SlimeInformationId { get; set; }
    [Name("SLIME_EXPRESSION")] public string SlimeExpressionId { get; set; }
    [Name("SLIME_ICON")] public string SlimeIconId { get; set; }
    [Name("LOCKED_ICON")] public string LockedIconId { get; set; }
    [Name("SLIME_STORY")] public string SlimeStoryId { get; set; }

    public string[] GetScriptIds()
    {
        return SlimeScriptId.Split('|').ToArray();
    }
    public override string ToString()
    {
        return $"{SlimeId} / {SlimeNameId} / {SlimeTypeId}  / {GiftItemId} / {SlimeExpressionId} / {SlimeScriptId} / {SlimeIconId} / {LockedIconId} / {SlimeStoryId}";
    }
}
#endregion

#region LevelUpData
[Serializable]
public class LevelUpData1 : ILevelUpData
{
    [Name("LEVELUP_ID")] public int LevelId { get; set; }
    [Name("SLIME_RARITY")] public int SlimeRarity { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType}";
    }
}
[Serializable]
public class LevelUpData2 : ILevelUpData
{
    [Name("LEVELUP_ID")] public int LevelId { get; set; }
    [Name("SLIME_RARITY")] public int SlimeRarity { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType}";
    }
}
[Serializable]
public class LevelUpData3 : ILevelUpData
{
    [Name("LEVELUP_ID")] public int LevelId { get; set; }
    [Name("SLIME_RARITY")] public int SlimeRarity { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType}";
    }
}
[Serializable]
public class LevelUpData4 : ILevelUpData
{
    [Name("LEVELUP_ID")] public int LevelId { get; set; }
    [Name("SLIME_RARITY")] public int SlimeRarity { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType}";
    }
}
#endregion

#region ItemData
[Serializable]
public class ItemData
{
    [Name("ITEM_ID")] public string ItemId { get; set; }
    [Name("ITEM_NAME")] public string ItemName { get; set; }
    [Name("OPTION_TYPE")] public int OptionType { get; set; }
    [Name("DEFAULT_VALUE")] public float DefaultValue { get; set; }
    [Name("MIN_VALUE")] public float MinValue { get; set; }
    [Name("MAX_VALUE")] public float MaxValue { get; set; }
    [Name("UNIT_VALUE")] public float UnitValue { get; set; }
    [Name("UI_TEXT_ITEM")] public string UIText { get; set; }
    [Name("ITEM_DESCRIPTION")] public string Description { get; set; }

    public override string ToString()
    {
        return $"{ItemId} / {ItemName} / {OptionType} / {DefaultValue} / {MinValue} / {MaxValue} / {UnitValue} / {UIText} / {Description}";
    }
}
#endregion

#region UnlockConditionData
[Serializable]
public class UnlockConditionData
{
    [Name("UNLOCK_ID")] public int UnlockId { get; set; }
    [Name("SLIME_ID")] public int SlimeId { get; set; }
    [Name("ITEM_ID")] public string ItemId { get; set; }
    [Name("PRIORITY")] public int Priority { get; set; }
    [Name("OPTION_TYPE")] public int OptionType { get; set; }
    [Name("OPTION_VALUE")] public float OptionValue { get; set; }   // 해금조건 옵션 값
    [Name("SUB_CONDITION")] public int SubCondition { get; set; }
    [Name("DISAPPEAR_OPTION_TYPE")] public int DisappearOptionType { get; set; }
    [Name("DISAPPEAR_OPTION_VALUE")] public int DisappearOptionValue { get; set; }  // 소멸조건 옵션값
    [Name("DISAPPEAR_SUB_CONDITION")] public int DisappearSubCondition { get; set; }
    [Name("UNLOCK_DESCRIPTION")] public string Description { get; set; }
    [Name("SLIME_WARNING_SCRIPT")] public string SlimeWarningScript { get; set; }
}
#endregion

#region StringData
[Serializable]
public class StringData
{
    [Name("STRING_KEY")] public string key { get; set; }
    [Name("TEXT(kor)")] public string Value { get; set; }
    public override string ToString()
    {
        return $"{key} / {Value}";
    }
}
#endregion