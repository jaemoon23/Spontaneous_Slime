using System;
using CsvHelper.Configuration.Attributes;

#region SlimeData
[Serializable]
public class SlimeData
{
    [Name("SLIME_ID")] public string SlimeId { get; set; }
    [Name("SLIME_NAME")] public string SlimeName { get; set; }
    [Name("SLIME_TYPE")] public int SlimeType { get; set; }
    [Name("UNLOCK_GROUP")] public int UnlockGroup { get; set; }
    [Name("LEVEL_GROUP")] public int LevelGroup { get; set; }
    [Name("GIFT_ITEM_ID")] public string GiftItemId { get; set; }
    [Name("SLIME_EXPRESSION")] public string SlimeExpression { get; set; }
    [Name("SLIME_SCRIPT")] public string SlimeScript { get; set; }
    [Name("SLIME_ICON")] public string SlimeIcon { get; set; }
    [Name("LOCKED_ICON")] public string LockedIcon { get; set; }
    [Name("SLIME_STORY")] public string SlimeStory { get; set; }
    public override string ToString()
    {
        return $"{SlimeId} / {SlimeName} / {SlimeType} / {UnlockGroup} / {LevelGroup} / {GiftItemId} / {SlimeExpression} / {SlimeScript} / {SlimeIcon} / {LockedIcon} / {SlimeStory}";
    }
}
#endregion

#region LevelUpData
[Serializable]
public class LevelUpData
{
    [Name("LEVELUP_ID")] public string LevelId { get; set; }
    [Name("SLIME_ID")] public string SlimeId { get; set; }
    [Name("LEVEL_GROUP")] public int LevelGroup { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {SlimeId} / {LevelGroup} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType}";
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
    [Name("UNLOCK_ID")] public string UnlockId { get; set; }
    [Name("UNLOCK_GROUP")] public int UnlockGroup { get; set; }
    [Name("SLIME_ID")] public string SlimeId { get; set; }
    [Name("ITEM_ID")] public string ItemId { get; set; }
    [Name("PRIORITY")] public int Priority { get; set; }
    [Name("OPTION_TYPE")] public int OptionType { get; set; }
    [Name("OPTION_VALUE")] public float OptionValue { get; set; }
    [Name("SUB_CONDITION")] public int SubCondition { get; set; }
    [Name("DISAPPEAR_OPTION_TYPE")] public int DisappearOptionType { get; set; }
    [Name("DISAPPEAR_OPTION_VALUE")] public int DisappearOptionValue { get; set; }
    [Name("DISAPPEAR_SUB_CONDITION")] public int DisappearSubCondition { get; set; }
    [Name("UNLOCK_DESCRIPTION")] public string Description { get; set; }

    public override string ToString()
    {
        return $"{UnlockId} / {UnlockGroup} / {SlimeId} / {ItemId} / {Priority} / {OptionType} / {OptionValue} / {SubCondition} / {DisappearOptionType} / {DisappearOptionValue} / {DisappearSubCondition} / {Description}";
    }
}
#endregion

#region StringData
[Serializable]
public class StringData
{
    [Name("STRING_KEY")] public string key { get; set; }
    [Name("STRING")] public string Value { get; set; }
    public override string ToString()
    {
        return $"{key} / {Value}";
    }
}
#endregion