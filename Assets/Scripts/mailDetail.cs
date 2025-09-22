using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    }
    
    public void SetMailInfo(string slimeName, int gold, string id)
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
        
        // 메일 내용 텍스트 설정
        if (mailContentText != null)
        {
            mailContentText.text = $"{slimeName} 슬라임이 편지를 보냈습니다!\n\n" +
                                   $"안녕하세요! 오늘도 좋은 하루 보내세요~\n" +
                                   $"작은 선물을 준비했어요!\n\n" +
                                   $"보상: {gold} 골드";
        }
    }
}
