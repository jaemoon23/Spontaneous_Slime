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
    public bool isMailOpen { get; private set; } = false;
    
    private void Start()
    {
        mailButton.onClick.AddListener(OpenMailPanel);
        closeMailButton.onClick.AddListener(CloseMailPanel);

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
    }
    private void CloseMailPanel()
    {
        mailPanel.SetActive(false);
    }
    
    private void OpenMailDetail(string slimeName, int gold, string mailId)
    {
        var mailPrefab = Resources.Load<GameObject>(Paths.Mail);
        var mailInstance = Instantiate(mailPrefab, canvas.transform);
        
        var mailDetailScript = mailInstance.GetComponent<mailDetail>();
        if (mailDetailScript != null)
        {
            mailDetailScript.SetMailManager(this);
            mailDetailScript.SetMailInfo(slimeName, gold, mailId);
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

        var mail = Resources.Load<GameObject>(Paths.MailButton);

        var mailInstance = Instantiate(mail, mailViewPanel.transform);
        var slimeData = DataTableManager.SlimeTable.Get(slimeId);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
        string slimeName = nameData != null ? nameData.Value : "???";
        int gold = 100;
        
        // 메일 고유 ID 생성 (날짜 + 슬라임ID 조합)
        string mailId = $"mail_{day}_{slimeId}";

        // 메일 UI에 슬라임 정보 설정
        SetMailContent(mailInstance, slimeData, slimeName, gold);
        
        // 메일 버튼 클릭 이벤트 연결
        var mailButton = mailInstance.GetComponent<Button>();
        if (mailButton != null)
        {
            mailButton.onClick.AddListener(() => OpenMailDetail(slimeName, gold, mailId));
        }

        Debug.Log($"{slimeName} 슬라임이 {day}일차 메일을 보냄! ({gold}골드 지급) - ID: {mailId}");
    }
    
    private void SetMailContent(GameObject mailInstance, SlimeData slimeData, string slimeName, int gold)
    {
        // 슬라임 아이콘 설정
        var image = mailInstance.GetComponentInChildren<Image>();
        if (image != null)
        {
            var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIconId);
            if (iconData != null)
            {
                Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
                if (iconSprite != null)
                {
                    image.sprite = iconSprite;
                }
            }
        }
        
        // 메일 텍스트 설정
        var textMeshPro = mailInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{slimeName}님의 편지\n+{gold} 골드";
        }
    }
}
