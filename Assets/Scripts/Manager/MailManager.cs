using System.Collections.Generic;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    private void OnEnable()
    {
        TimeManager.OnDayPassed += Mail;
    }

    private void OnDisable()
    {
        TimeManager.OnDayPassed -= Mail;
    }

    private void Mail(int day)
    {
        // 도감에 등록된 슬라임 ID 리스트 가져오기
        List<int> collectedSlimeIds = SaveLoadManager.Data.CollectedSlimeIds;
        if (collectedSlimeIds == null || collectedSlimeIds.Count == 0)
        {
            Debug.Log("도감에 등록된 슬라임이 없습니다. 메일 발송 스킵.");
            return;
        }

        // 랜덤으로 한 마리 선택
        int randomIndex = Random.Range(0, collectedSlimeIds.Count);
        int slimeId = collectedSlimeIds[randomIndex];

        // 슬라임 이름 가져오기
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
        string slimeName = nameData != null ? nameData.Value : "???";
        int gold = 100;
        Debug.Log($"{slimeName} 슬라임이 {day}일차 메일을 보냄! ({gold}골드 지급)");
        CurrencyManager.Instance.AddGold(gold);
    }
}
