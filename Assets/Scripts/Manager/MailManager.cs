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

    private int normalTitleIndex = 0;
    private int darkTitleIndex = 0;
    private int lightTitleIndex = 0;
    private int fireTitleIndex = 0;
    private int iceTitleIndex = 0;
    private int waterTitleIndex = 0;
    private int plantTitleIndex = 0;
    private int rainTitleIndex = 0;
    private int catTitleIndex = 0;
    private int auroraTitleIndex = 0;


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
        UISoundManager.Instance.PlayOpenSound();
        isMailOpen = false;
        
        // 기존 메일들의 읽음 상태 업데이트
        UpdateMailReadStatus();
        
        // YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    private void CloseMailPanel()
    {
        mailPanel.SetActive(false);
        UISoundManager.Instance.PlayCloseSound();
        
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
    // 매일 메일 발송
    private void Mail(int day)
    {
        // 도감에 등록된 슬라임 ID 리스트 가져오기
        List<int> collectedSlimeIds = SaveLoadManager.Data.CollectedSlimeIds;
        if (collectedSlimeIds == null || collectedSlimeIds.Count == 0)
        {
            Debug.Log("도감에 등록된 슬라임이 없습니다. 메일 발송 스킵.");
            return;
        }

        // 편지를 보낼 수 있는 슬라임 찾기 (3개 편지를 모두 보내지 않은 슬라임)
        List<int> availableSlimes = new List<int>();
        foreach (int collectedSlimeId in collectedSlimeIds)
        {
            // 슬라임의 현재 편지 인덱스 확인 (없으면 0으로 초기화)
            if (!SaveLoadManager.Data.SlimeLetterIndex.ContainsKey(collectedSlimeId))
            {
                SaveLoadManager.Data.SlimeLetterIndex[collectedSlimeId] = 0;
            }
            
            // 아직 3개 편지를 모두 보내지 않은 슬라임만 추가
            if (SaveLoadManager.Data.SlimeLetterIndex[collectedSlimeId] < 3)
            {
                availableSlimes.Add(collectedSlimeId);
            }
        }

        // 편지를 보낼 수 있는 슬라임이 없으면 종료
        if (availableSlimes.Count == 0)
        {
            Debug.Log("모든 슬라임이 편지를 완료했습니다. 메일 발송 스킵.");
            return;
        }

        // 랜덤으로 한 마리 선택
        int randomIndex = Random.Range(0, availableSlimes.Count);
        int slimeId = availableSlimes[randomIndex];
        int letterIndex = SaveLoadManager.Data.SlimeLetterIndex[slimeId];

        // 메일 고유 ID 생성 (슬라임ID + 편지인덱스 조합)
        string mailId = $"mail_{slimeId}_{letterIndex}";

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
        int gold = slimeData.LetterEther;

        // 메일 UI에 슬라임 정보 설정
        SetMailContent(mailInstance, slimeData, slimeName, gold, mailId);

        // 메일 버튼 클릭 이벤트 연결
        var mailButton = mailInstance.GetComponent<Button>();
        if (mailButton != null)
        {
            mailButton.onClick.AddListener(() => OpenMailDetail(slimeId, gold, mailId));
        }

        // 해당 슬라임의 편지 인덱스 증가
        SaveLoadManager.Data.SlimeLetterIndex[slimeId]++;

        Debug.Log($"{slimeName} 슬라임이 {letterIndex + 1}번째 편지를 보냄! ({gold}에테르 지급) - ID: {mailId}");

        // 새 메일이 도착했으므로 YellowDot 상태 업데이트
        UpdateYellowDotStatus();
    }
    
    // 메일 UI에 슬라임 정보 설정
    private void SetMailContent(GameObject mailInstance, SlimeData slimeData, string slimeName, int gold, string mailId)
    {
        // 읽음 상태 확인
        bool isRead = SaveLoadManager.Data.ReadMailIds.Contains(mailId);
        
        // 메일 ID에서 편지 인덱스 추출 (mail_slimeId_letterIndex)
        string[] mailParts = mailId.Split('_');
        int letterIndex = 0;
        if (mailParts.Length >= 3 && int.TryParse(mailParts[2], out int parsedIndex))
        {
            letterIndex = parsedIndex;
        }
        
        // 편지 제목 가져오기
        string letterTitle = slimeName + "님의 편지"; // 기본값
        var letterTitleIds = slimeData.GetLetterTitleIds();
        if (letterIndex >= 0 && letterIndex < letterTitleIds.Length)
        {
            var titleData = DataTableManager.StringTable.Get(letterTitleIds[letterIndex]);
            if (titleData != null)
            {
                letterTitle = titleData.Value;
            }
        }

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

        // 메일 텍스트 설정 (편지 제목 사용)
        var textMeshPro = mailInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{letterTitle}\n+{gold} 에테르";
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
        {
            return;
        }
        
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
        bool hasUnreadMails = HasUnreadMails();
        bool hasUnclaimedRewards = HasUnclaimedRewards();
        bool shouldShow = hasUnreadMails || hasUnclaimedRewards;
        
        Debug.Log($"YellowDot 상태 확인 - 읽지 않은 메일: {hasUnreadMails}, 받지 않은 보상: {hasUnclaimedRewards}, 표시 여부: {shouldShow}");
        
        if (YellowDot != null)
        {
            YellowDot.SetActive(shouldShow);
            Debug.Log($"YellowDot 설정됨: {shouldShow}");
        }
        else
        {
            Debug.LogError("YellowDot GameObject가 null입니다! Inspector에서 연결을 확인해주세요.");
        }
    }
    
    // 읽지 않은 메일이 있는지 확인
    private bool HasUnreadMails()
    {
        int unreadCount = 0;
        foreach (Transform child in mailViewPanel.transform)    // mailViewPanel의 모든 자식 검사
        {
            var mailIdentifier = child.GetComponent<MailIdentifier>();  // MailId를 저장하는 컴포넌트
            if (mailIdentifier != null && !string.IsNullOrEmpty(mailIdentifier.MailId)) // MailId가 유효한지 확인
            {
                // ReadMailIds에 없으면 읽지 않은 메일
                if (!SaveLoadManager.Data.ReadMailIds.Contains(mailIdentifier.MailId))
                {
                    unreadCount++;
                }
            }
        }
        Debug.Log($"읽지 않은 메일 개수: {unreadCount}");
        return unreadCount > 0;
    }
    
    // 보상 안받은 메일이 있는지 확인
    private bool HasUnclaimedRewards()
    {
        int unclaimedCount = 0;
        foreach (Transform child in mailViewPanel.transform)    // mailViewPanel의 모든 자식 검사
        {
            var mailIdentifier = child.GetComponent<MailIdentifier>();  // MailId를 저장하는 컴포넌트
            if (mailIdentifier != null && !string.IsNullOrEmpty(mailIdentifier.MailId)) // MailId가 유효한지 확인
            {
                // ReceivedMailIds에 없으면 보상을 안받은 메일
                if (!SaveLoadManager.Data.ReceivedMailIds.Contains(mailIdentifier.MailId))
                {
                    unclaimedCount++;
                }
            }
        }
        Debug.Log($"보상을 받지 않은 메일 개수: {unclaimedCount}");
        return unclaimedCount > 0;
    }
    
    // 외부에서 YellowDot 상태 업데이트
    public void RefreshYellowDotStatus()
    {
        UpdateYellowDotStatus();
    }
    
    // 저장된 메일 데이터를 기반으로 메일 UI 복원
    public void LoadMailUI()
    {
        // 기존 메일 UI 모두 제거
        foreach (Transform child in mailViewPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        var saveData = SaveLoadManager.Data;
        if (saveData.SlimeLetterIndex == null || saveData.SlimeLetterIndex.Count == 0)
        {
            Debug.Log("저장된 메일 데이터가 없습니다.");
            return;
        }
        
        // 각 슬라임별로 보낸 편지들을 다시 생성
        foreach (var kvp in saveData.SlimeLetterIndex)
        {
            int slimeId = kvp.Key;
            int currentLetterIndex = kvp.Value;
            
            // 현재 편지 인덱스까지 모든 편지 생성 (0부터 currentLetterIndex-1까지)
            for (int letterIndex = 0; letterIndex < currentLetterIndex; letterIndex++)
            {
                CreateMailUI(slimeId, letterIndex);
            }
        }
        
        // 메일 읽음 상태 업데이트
        UpdateMailReadStatus();
        
        // YellowDot 상태 업데이트
        UpdateYellowDotStatus();
        
        Debug.Log($"메일 UI 복원 완료: {saveData.SlimeLetterIndex.Count}명의 슬라임 메일 로드");
    }
    
    // 개별 메일 UI 생성
    private void CreateMailUI(int slimeId, int letterIndex)
    {
        // 메일 고유 ID 생성
        string mailId = $"mail_{slimeId}_{letterIndex}";
        
        var mail = Resources.Load<GameObject>(Paths.MailButton);
        if (mail == null)
        {
            Debug.LogError("메일 프리팹을 찾을 수 없습니다!");
            return;
        }
        
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
        if (slimeData == null)
        {
            Debug.LogError($"슬라임 데이터를 찾을 수 없습니다: {slimeId}");
            Destroy(mailInstance);
            return;
        }
        
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
        string slimeName = nameData != null ? nameData.Value : "???";
        int gold = slimeData.LetterEther;
        
        // 메일 UI에 슬라임 정보 설정
        SetMailContent(mailInstance, slimeData, slimeName, gold, mailId);
        
        // 메일 버튼 클릭 이벤트 연결
        var mailButton = mailInstance.GetComponent<Button>();
        if (mailButton != null)
        {
            mailButton.onClick.AddListener(() => OpenMailDetail(slimeId, gold, mailId));
        }
        
        Debug.Log($"{slimeName} 슬라임의 {letterIndex + 1}번째 편지 UI 생성 - ID: {mailId}");
    }
}
