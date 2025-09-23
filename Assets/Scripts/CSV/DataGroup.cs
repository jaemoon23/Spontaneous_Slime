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
    int LevelUpEther { get; set; }
}

#region SlimeData
[Serializable]
public class SlimeData
{
    [Name("SLIME_ID")] public int SlimeId { get; set; }
    [Name("SLIME_NAME")] public string SlimeNameId { get; set; }
    [Name("SLIME_TYPE")] public int SlimeTypeId { get; set; }
    [Name("RARITY")] public int RarityId { get; set; }
    [Name("GIFT_ITEM_ID")] public int GiftItemId { get; set; }
    [Name("SLIME_SCRIPT")] public string SlimeScriptId { get; set; }
    [Name("SLIME_INFORMATION")] public string SlimeInformationId { get; set; }
    [Name("SLIME_EXPRESSION")] public string SlimeExpressionId { get; set; }
    [Name("SLIME_ICON")] public string SlimeIconId { get; set; }
    [Name("LOCKED_ICON")] public string LockedIconId { get; set; }
    [Name("SLIME_STORY")] public string SlimeStoryId { get; set; }
    [Name("LETTER")] public string LetterId { get; set; }

    public string[] GetScriptIds()
    {
        return SlimeScriptId.Split('|').ToArray();
    }
    public string SlimeName
    {
        get
        {
            var nameData = DataTableManager.StringTable.Get(SlimeNameId);
            return nameData != null ? nameData.Value : "Unknown";
        }
    }

    public string[] GetLetterIds()
    {
        return LetterId.Split('|').ToArray();
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
    [Name("LEVELUP_EHTER")] public int LevelUpEther { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType} / {LevelUpEther}";
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
    [Name("LEVELUP_EHTER")] public int LevelUpEther { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType} / {LevelUpEther}";
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
    [Name("LEVELUP_EHTER")] public int LevelUpEther { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType} / {LevelUpEther}";
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
    [Name("LEVELUP_EHTER")] public int LevelUpEther { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType} / {LevelUpEther}";
    }
}
[Serializable]
public class LevelUpData5 : ILevelUpData
{
    [Name("LEVELUP_ID")] public int LevelId { get; set; }
    [Name("SLIME_RARITY")] public int SlimeRarity { get; set; }
    [Name("CURRENT_LEVEL")] public int CurrentLevel { get; set; }
    [Name("NEED_EXP")] public int NeedExp { get; set; }
    [Name("SCALE_LEVEL")] public int ScaleLevel { get; set; }
    [Name("EVENT_TYPE")] public int EventType { get; set; }
    [Name("LEVELUP_EHTER")] public int LevelUpEther { get; set; }
    public override string ToString()
    {
        return $"{LevelId} / {CurrentLevel} /  {NeedExp}  {ScaleLevel} / {EventType} / {LevelUpEther}";
    }
}
#endregion

#region InteriorData
[Serializable]
public class InteriorData
{
    [Name("INTERIOR_ID")] public int InteriorId { get; set; }
    [Name("INTERIOR_NAME")] public string InteriorName { get; set; }
    [Name("OPTION_TYPE")] public int OptionType { get; set; }
    [Name("DEFAULT_VALUE")] public int DefaultValue { get; set; }
    [Name("MIN_VALUE")] public float MinValue { get; set; }
    [Name("MAX_VALUE")] public float MaxValue { get; set; }
    [Name("UNIT_VALUE")] public float UnitValue { get; set; }
    [Name("UI_TEXT_INTERIOR")] public string UIText { get; set; }
    [Name("INTERIOR_DESCRIPTION")] public string Description { get; set; }

    public override string ToString()
    {
        return $"{InteriorId} / {InteriorName} / {OptionType} / {DefaultValue} / {MinValue} / {MaxValue} / {UnitValue} / {UIText} / {Description}";
    }
}
#endregion

#region ItemData
[Serializable]
public class ItemData
{
    [Name("ITEM_ID")] public int ItemId { get; set; }
    [Name("ITEM_NAME")] public string ItemName { get; set; }
    [Name("ITEM_TYPE")] public int ItemType { get; set; }
    [Name("OPTION_TYPE")] public int ItemOptionType { get; set; }
    [Name("OPTION_VALUE")] public int ItemOptionValue { get; set; }
    [Name("MAX_STACK")] public int MaxStack { get; set; }
    [Name("1_TIME_MAX_QTY")] public int OneMaxQty { get; set; }
    [Name("USE_CONDITION")] public int UseCondition { get; set; }
    [Name("ITEM_ICON")] public string ItemIcon { get; set; }
    [Name("ITEM_DESCRIPTION")] public string Description { get; set; }

    
    public override string ToString()
    {
        return $"{ItemId} / {ItemName} / {ItemType} / {ItemOptionType} / {ItemOptionValue} / {MaxStack} / {OneMaxQty} / {UseCondition} / {ItemIcon} / {Description}";
    }
}
#endregion

#region StoreData
[Serializable]
public class StoreData
{
    [Name("PRODUCT_ID")] public int productId { get; set; }
    [Name("PRODUCT_NAME")] public string productName { get; set; }
    [Name("PRODUCT_TYPE")] public int productType { get; set; }
    [Name("PRICE")] public int price { get; set; }
    [Name("BUY_LIMIT")] public int buyLimit { get; set; }
    [Name("BUY_CONDITION")] public int buyCondition { get; set; }
    [Name("1_TIME_MAX_QTY")] public int oneTimeMaxQty { get; set; }
    

    public override string ToString()
    {
        return $"{productId} / {productName} / {productType} / {price} / {buyLimit} / {buyCondition} / {oneTimeMaxQty}";
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