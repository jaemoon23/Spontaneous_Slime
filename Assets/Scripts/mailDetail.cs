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
    [SerializeField] private TextMeshProUGUI mailEtherText; // 에테르 텍스트
    
    private string mailId; // 메일의 고유 ID
    private int etherAmount = 0; // 받을 에테르 양

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
        
        // 에테르 지급
        Debug.Log("에테르를 받았습니다!");
        CurrencyManager.Instance.AddEther(etherAmount);
        
        // 저장
        SaveLoadManager.Save();
        
        // YellowDot 상태 업데이트
        if (mailManager != null)
        {
            mailManager.RefreshYellowDotStatus();
        }
    }
    
    public void SetMailInfo(int slimeId, int ether, string id)
    {
        mailId = id;
        etherAmount = ether;
        
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

            // 메일 ID에서 편지 인덱스 추출 (mail_slimeId_letterIndex)
            string[] mailParts = mailId.Split('_');
            int letterIndex = 0;
            if (mailParts.Length >= 3 && int.TryParse(mailParts[2], out int parsedIndex))
            {
                letterIndex = parsedIndex;
            }

            // 편지 내용을 인덱스 기반으로 가져오기
            var letterIds = slimeData.GetLetterIds();
            var letterTitleIds = slimeData.GetLetterTitleIds();
            var letterContent = string.Empty;
            var letterTitle = string.Empty;

            // 인덱스 범위 확인 후 해당 인덱스의 편지 가져오기
            if (letterIndex >= 0 && letterIndex < letterIds.Length)
            {
                // 편지 내용
                var letterData = DataTableManager.StringTable.Get(letterIds[letterIndex]);
                if (letterData != null)
                {
                    letterContent = letterData.Value;
                }
            }

            // 편지 내용 표시
            mailContentText.text = $"{letterContent}";
            mailEtherText.text = $"{etherAmount} 에테르"; // 에테르 양 표시
           
        }
    }
}
