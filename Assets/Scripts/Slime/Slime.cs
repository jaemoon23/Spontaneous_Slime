using UnityEngine;

public enum SlimeType
{
    Normal,
    Light,
    Dark,
    Water,
    Ice,
    Fire,
    Plant,
}
// TDOD: Debug.Log 제거 및 주석 정리
public class Slime : MonoBehaviour
{
    private string expressions; // 표정
    private string stringScripts;   // 대사
    private string stringScriptsId;   // 대사
    private string slimeNameId; // 슬라임 이름 ID
    public string slimeName; // 슬라임 이름

    //TDOD: 아이콘 관련 기능 추가 필요
    private Sprite icon;    // 아이콘
    public string GiftId { get; private set; }
    [SerializeField] private SlimeType slimeType = SlimeType.Normal;

    private void Start()
    {
        // 슬라임 타입 설정
        //slimeType = (SlimeType)type;

        // 슬라임 데이터 가져오기
        var slimeData = DataTableManager.SlimeTable.Get(DataTableIds.SlimeIds[(int)slimeType]);
        Debug.Log($"슬라임 타입: {slimeType}, 데이터 ID: {(int)slimeType}");
        if (slimeData != null)
        {
            slimeNameId = slimeData.SlimeName; // 슬라임 이름 ID
            GiftId = slimeData.GiftItemId;     // 선물 아이템 ID
            stringScriptsId = slimeData.SlimeScript; // 대사 ID
            expressions = slimeData.SlimeExpression; // 표정 ID

            // 문자열 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(slimeNameId);
            var stringScriptsData = DataTableManager.StringTable.Get(stringScriptsId);
            if (stringData != null || stringScriptsData != null)
            {
                slimeName = stringData.Value;            // 슬라임 이름
                stringScripts = stringScriptsData.Value; // 대사 ID
                Debug.Log($"슬라임 이름: {slimeName}");
            }

            Debug.Log($"슬라임 데이터 로드 완료: {slimeNameId}, 선물 아이템 ID: {GiftId}");
            Debug.Log($"표정: {expressions}, 스크립트: {stringScripts}");
        }
    }
}
