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
    
    private bool isTake = false; // 이미 받았는지 확인하는 플래그
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
        if (mailManager != null)
        {
            mailManager.OpenMailPanel(); // 메일 패널 다시 활성화
        }
        Destroy(gameObject); // 메일 상세 창 닫기
    }

    private void TakeMail()
    {
        if (isTake) return; // 이미 받았으면 리턴
        
        isTake = true; // 받음 표시
        takeButton.interactable = false; // 버튼 비활성화
        
        // 여기서 골드 지급 로직을 추가할 수 있습니다.
        Debug.Log("골드를 받았습니다!");
        CurrencyManager.Instance.AddGold(goldAmount);
    }
    
    public void SetMailInfo(string slimeName, int gold)
    {
        
        goldAmount = gold;
        
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
