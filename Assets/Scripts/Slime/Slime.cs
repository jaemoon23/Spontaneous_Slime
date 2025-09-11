using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum SlimeType
{
    Normal,
    Light,
    Dark ,
    Water,
    Ice,
    Fire,
    Plant,
}

public class Slime : MonoBehaviour
{
    //TODO: 슬라임 관련 기능 구현
    private List<SlimeData> slimeDataList = new List<SlimeData>(); // 슬라임 데이터 리스트
    public IReadOnlyList<StringData> StringDataList { get; private set; } = new List<StringData>();
    private string expressions;
    private string stringScripts;
    private string slimeName;
    private Sprite icon;
    public string GiftId { get; private set; } 
    [SerializeField] private int type;
    private SlimeType slimeType= SlimeType.Normal; // 슬라임 타입
    private void Start()
    {
        // 슬라임 데이터 가져오기
        foreach (var id in DataTableIds.SlimeIds)
        {
            var slimeData = DataTableManager.SlimeTable.Get(id);
            slimeDataList.Add(slimeData);
        }

        // 문자열 데이터 가져오기
        var tempList = new List<StringData>();
        foreach (var key in DataTableIds.StringKeys)
        {
            var stringData = DataTableManager.StringTable.Get(key);
            tempList.Add(stringData);
        }
        StringDataList = tempList;

        switch (type)
        {
            case 0:
                slimeType = SlimeType.Normal;
                break;
            case 1:
                slimeType = SlimeType.Light;
                break;
            case 2:
                slimeType = SlimeType.Dark;
                break;
            case 3:
                slimeType = SlimeType.Water;
                break;
            case 4:
                slimeType = SlimeType.Ice;
                break;
            case 5:
                slimeType = SlimeType.Fire;
                break;
            case 6:
                slimeType = SlimeType.Plant;
                break;
            default:
                Debug.LogWarning("Invalid slime type");
                break;
        }

        // 슬라임 표정
        Debug.Log($"2표정: {slimeDataList[(int)slimeType].SlimeExpression}");
        expressions = slimeDataList[(int)slimeType].SlimeExpression;

        // 선물 아이템 ID
        GiftId = slimeDataList[(int)slimeType].GiftItemId;
        Debug.Log($"선물 아이템 ID: {GiftId}");

        //TODO: 나중에 열거형 값으로 인덱스 채우기
        if (StringDataList[(int)slimeType] != null)
        {
            foreach (var stringData in StringDataList)
            {
                // 슬라임 스크립트
                if (stringData.key == slimeDataList[(int)slimeType].SlimeScript)
                {
                    Debug.Log($"1키: {stringData.key}, 문자열: {stringData.Value}");
                    stringScripts = stringData.Value;
                }
                // 슬라임 이름
                if (stringData.key == slimeDataList[(int)slimeType].SlimeName)
                {
                    Debug.Log($"3키: {stringData.key}, 문자열: {stringData.Value}");
                    slimeName = stringData.Value;
                }
            }
        }
    }
}
