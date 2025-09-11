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
    private List<StringData> stringDataList = new List<StringData>(); // 문자열 데이터 리스트

    private string expressions;
    private string stringScripts;
    private string slimeName;
    private Sprite icon;
    [SerializeField] private int type;
    private void Start()
    {
        // 슬라임 데이터 가져오기
        foreach (var id in DataTableIds.SlimeIds)
        {
            var slimeData = DataTableManager.SlimeTable.Get(id);
            slimeDataList.Add(slimeData);
        }

        // 문자열 데이터 가져오기
        foreach (var key in DataTableIds.StringKeys)
        {
            var stringData = DataTableManager.StringTable.Get(key);
            stringDataList.Add(stringData);
        }

        // 슬라임 표정
        Debug.Log($"2표정: {slimeDataList[(int)SlimeType.Normal].SlimeExpression}");
        expressions = slimeDataList[(int)SlimeType.Normal].SlimeExpression;

        //TODO: 나중에 열거형 값으로 인덱스 채우기
        if (stringDataList[(int)SlimeType.Normal] != null)
        {
            foreach (var stringData in stringDataList)
            {
                // 슬라임 스크립트
                if (stringData.key == slimeDataList[(int)SlimeType.Normal].SlimeScript)
                {
                    Debug.Log($"1키: {stringData.key}, 문자열: {stringData.Value}");
                    stringScripts = stringData.Value;
                }
                // 슬라임 이름
                if (stringData.key == slimeDataList[(int)SlimeType.Normal].SlimeName)
                {
                    Debug.Log($"3키: {stringData.key}, 문자열: {stringData.Value}");
                    slimeName = stringData.Value;
                }
            }
        }
    }
}
