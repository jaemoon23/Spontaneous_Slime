using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class mailDetail : MonoBehaviour
{
    private GameObject mailPanel; // 메일 패널
    private MailManager mailManager;
    [SerializeField] private Button yesButton; // 확인 버튼
    [SerializeField] private Button takeButton; // 받기 버튼
    [SerializeField] private TextMeshProUGUI mailContentText; // 메일 내용 텍스트
    
    private string mailId; // 메일의 고유 ID
    private int goldAmount = 0; // 받을 골드 양

    private void Start()
    {
        yesButton.onClick.AddListener(CloseMailDetail);
        takeButton.onClick.AddListener(TakeMail);
    }
    

    public void SetMailManager(MailManager manager)
    {
        mailManager = manager;
    }

    private void CloseMailDetail()
    {
        // 메일을 읽음 처리
        if (!string.IsNullOrEmpty(mailId) && !SaveLoadManager.Data.ReadMailIds.Contains(mailId))
        {
            SaveLoadManager.Data.ReadMailIds.Add(mailId);
            SaveLoadManager.Save();
            Debug.Log($"메일 {mailId} 읽음 처리됨");
        }
        
        if (mailManager != null)
        {
            mailManager.OpenMailPanel(); // 메일 패널 다시 활성화
            // YellowDot 상태 업데이트
            mailManager.RefreshYellowDotStatus();
        }
        Destroy(gameObject); // 메일 상세 창 닫기
    }

    private void TakeMail()
    {
        // 이미 받았는지 SaveData에서 확인
        if (SaveLoadManager.Data.ReceivedMailIds.Contains(mailId)) return;
        
        // 받은 메일 ID를 SaveData에 추가
        SaveLoadManager.Data.ReceivedMailIds.Add(mailId);
        takeButton.interactable = false; // 버튼 비활성화
        
        // 골드 지급
        Debug.Log("골드를 받았습니다!");
        CurrencyManager.Instance.AddGold(goldAmount);
        
        // 저장
        SaveLoadManager.Save();
        
        // YellowDot 상태 업데이트
        if (mailManager != null)
        {
            mailManager.RefreshYellowDotStatus();
        }
    }
    
    public void SetMailInfo(int slimeId, int gold, string id)
    {
        mailId = id;
        goldAmount = gold;
        
        // 이미 받았는지 확인하여 버튼 상태 설정
        bool alreadyReceived = SaveLoadManager.Data.ReceivedMailIds.Contains(mailId);
        if (alreadyReceived)
        {
            takeButton.interactable = false;
            takeButton.GetComponentInChildren<TextMeshProUGUI>().text = "이미 받음";
        }
        
        // 슬라임 데이터에서 이름과 편지 내용 가져오기
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        if (slimeData != null && mailContentText != null)
        {
            var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
            string slimeName = nameData != null ? nameData.Value : "Unknown";
            
            // 편지 내용을 슬라임 데이터에서 가져오기
            var letterIds = slimeData.GetLetterIds();
            var letterContent = string.Empty;
            
            // 이미 저장된 메일 내용이 있는지 확인
            if (SaveLoadManager.Data.MailContents == null)
            {
                SaveLoadManager.Data.MailContents = new Dictionary<string, string>();
            }
            
            if (SaveLoadManager.Data.MailContents.ContainsKey(mailId))
            {
                // 이미 저장된 내용 사용
                letterContent = SaveLoadManager.Data.MailContents[mailId];
            }
            else
            {
                // 처음 생성하는 메일이므로 랜덤 선택 후 저장
                if (letterIds.Length > 0)
                {
                    var letterData = DataTableManager.StringTable.Get(letterIds[Random.Range(0, letterIds.Length)]);
                    if (letterData != null)
                    {
                        letterContent = letterData.Value;
                        // 선택된 내용을 저장
                        SaveLoadManager.Data.MailContents[mailId] = letterContent;
                        SaveLoadManager.Save();
                    }
                }
            }

            mailContentText.text = $"{letterContent}";

        }
    }
}
