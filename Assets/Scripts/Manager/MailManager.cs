using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailManager : MonoBehaviour
{
    [SerializeField] private Button mailButton;
    [SerializeField] private Button closeMailButton;
    [SerializeField] private GameObject mailPanel; // 메일 패널
    [SerializeField] private GameObject mailViewPanel; // 메일 뷰 패널
    [SerializeField] private GameObject canvas; // 메일 상세 패널 
    [SerializeField] private GameObject YellowDot;
    public bool isMailOpen { get; private set; } = false;
    
    private void Start()
    {
        mailButton.onClick.AddListener(OpenMailPanel);
        closeMailButton.onClick.AddListener(CloseMailPanel);
        
        // 시작할 때 YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    private void OnEnable()
    {
        TimeManager.OnDayPassed += Mail;
    }

    private void OnDisable()
    {
        TimeManager.OnDayPassed -= Mail;
    }

    public void OpenMailPanel()
    {
        mailPanel.SetActive(true);
        isMailOpen = false;
        
        // 기존 메일들의 읽음 상태 업데이트
        UpdateMailReadStatus();
        
        // YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    private void CloseMailPanel()
    {
        mailPanel.SetActive(false);
        
        // YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    
    private void OpenMailDetail(int slimeId, int gold, string mailId)
    {
        var mailPrefab = Resources.Load<GameObject>(Paths.Mail);
        var mailInstance = Instantiate(mailPrefab, canvas.transform);
        
        var mailDetailScript = mailInstance.GetComponent<mailDetail>();
        if (mailDetailScript != null)
        {
            mailDetailScript.SetMailManager(this);
            mailDetailScript.SetMailInfo(slimeId, gold, mailId);
            isMailOpen = true;
        }
        
        mailPanel.SetActive(false);
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

        // 메일 고유 ID 생성 (날짜 + 슬라임ID 조합)
        string mailId = $"mail_{day}_{slimeId}";

        var mail = Resources.Load<GameObject>(Paths.MailButton);

        var mailInstance = Instantiate(mail, mailViewPanel.transform);
        mailInstance.transform.SetSiblingIndex(0);
        
        // 메일 ID를 저장할 컴포넌트 추가
        var mailIdentifier = mailInstance.GetComponent<MailIdentifier>();
        if (mailIdentifier == null)
        {
            mailIdentifier = mailInstance.AddComponent<MailIdentifier>();
        }
        mailIdentifier.MailId = mailId;
        
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
        string slimeName = nameData != null ? nameData.Value : "???";
        int gold = 100;

        // 메일 UI에 슬라임 정보 설정
        SetMailContent(mailInstance, slimeData, slimeName, gold, mailId);
        
        // 메일 버튼 클릭 이벤트 연결
        var mailButton = mailInstance.GetComponent<Button>();
        if (mailButton != null)
        {
            mailButton.onClick.AddListener(() => OpenMailDetail(slimeId, gold, mailId));
        }

        Debug.Log($"{slimeName} 슬라임이 {day}일차 메일을 보냄! ({gold}골드 지급) - ID: {mailId}");
        
        // 새 메일이 도착했으므로 YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    
    private void SetMailContent(GameObject mailInstance, SlimeData slimeData, string slimeName, int gold, string mailId)
    {
        // 읽음 상태 확인
        bool isRead = SaveLoadManager.Data.ReadMailIds.Contains(mailId);
        
        // 슬라임 아이콘 설정 (색상 변경 없이 스프라이트만 설정)
        var slimeIconImage = mailInstance.GetComponentInChildren<Image>();
        if (slimeIconImage != null)
        {
            var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIconId);
            if (iconData != null)
            {
                Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
                if (iconSprite != null)
                {
                    slimeIconImage.sprite = iconSprite;
                    // 슬라임 아이콘은 항상 원본 색상 유지
                    slimeIconImage.color = Color.white;
                }
            }
        }
        
        // 배경 이미지들의 색상 변경
        var allImages = mailInstance.GetComponentsInChildren<Image>();
        foreach (var image in allImages)
        {
            // 슬라임 아이콘이 아닌 다른 이미지들 (배경 이미지 등)
            if (image != slimeIconImage)
            {
                if (isRead)
                {
                    image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 어둡게
                }
                else
                {
                    image.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
        
        // 메일 텍스트 설정
        var textMeshPro = mailInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{slimeName}님의 편지\n+{gold} 골드";
            textMeshPro.color = Color.black; // 검은색
        }
    }
    
    // 기존 메일들의 읽음 상태 업데이트
    private void UpdateMailReadStatus()
    {
        // mailViewPanel의 모든 자식 메일 버튼들을 찾아서 읽음 상태 업데이트
        foreach (Transform child in mailViewPanel.transform)
        {
            var mailButton = child.GetComponent<Button>();
            if (mailButton != null)
            {
                UpdateSingleMailDisplay(child.gameObject);
            }
        }
    }
    
    // 개별 메일의 표시 상태 업데이트
    private void UpdateSingleMailDisplay(GameObject mailInstance)
    {
        var mailIdentifier = mailInstance.GetComponent<MailIdentifier>();
        if (mailIdentifier == null || string.IsNullOrEmpty(mailIdentifier.MailId))
            return;
        
        bool isRead = SaveLoadManager.Data.ReadMailIds.Contains(mailIdentifier.MailId);
        
        // 배경 이미지 색상 변경 (텍스트 배경)
        var images = mailInstance.GetComponentsInChildren<Image>();
        var slimeIconImage = mailInstance.GetComponentInChildren<Image>(); // 첫 번째 이미지는 슬라임 아이콘
        
        foreach (var image in images)
        {
            // 배경 이미지
            if (image != slimeIconImage)
            {
                if (isRead)
                {
                    image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 어둡게
                }
                else
                {
                    image.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
        
        // 슬라임 아이콘은 항상 원본 색상 유지
        if (slimeIconImage != null)
        {
            slimeIconImage.color = Color.white;
        }
        
        // 텍스트 색상 변경
        var texts = mailInstance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            text.color = Color.black; // 검은색
        }
    }
    
    // YellowDot 상태 업데이트 메서드
    private void UpdateYellowDotStatus()
    {
        bool shouldShow = HasUnreadMails() || HasUnclaimedRewards();
        
        if (YellowDot != null)
        {
            YellowDot.SetActive(shouldShow);
        }
    }
    
    // 읽지 않은 메일이 있는지 확인
    private bool HasUnreadMails()
    {
        foreach (Transform child in mailViewPanel.transform)
        {
            var mailIdentifier = child.GetComponent<MailIdentifier>();
            if (mailIdentifier != null && !string.IsNullOrEmpty(mailIdentifier.MailId))
            {
                // ReadMailIds에 없으면 읽지 않은 메일
                if (!SaveLoadManager.Data.ReadMailIds.Contains(mailIdentifier.MailId))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // 보상 안받은 메일이 있는지 확인
    private bool HasUnclaimedRewards()
    {
        foreach (Transform child in mailViewPanel.transform)
        {
            var mailIdentifier = child.GetComponent<MailIdentifier>();
            if (mailIdentifier != null && !string.IsNullOrEmpty(mailIdentifier.MailId))
            {
                // ReceivedMailIds에 없으면 보상을 안받은 메일
                if (!SaveLoadManager.Data.ReceivedMailIds.Contains(mailIdentifier.MailId))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // 외부에서 YellowDot 상태 업데이트를 요청할 수 있는 public 메서드
    public void RefreshYellowDotStatus()
    {
        UpdateYellowDotStatus();
    }
}
