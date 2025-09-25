using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class CollectionSlot : MonoBehaviour
{
    CollectionManager collectionManager;
    private GameObject collectionManagerObject; // 컬렉션 매니저 오브젝트 참조
    public int SlimeId { get; private set; }
    [SerializeField] private Image slimeIcon;
    [SerializeField] private TextMeshProUGUI slimeNameText;
    private DateTime collectionTime;
    private GameObject collectionPanel; // 슬라임 도감 패널

    private GameObject uiManagerObject; // UI 매니저 오브젝트 참조
    private UiManager uiManager;

    private CollectionManager colManager;
    private GameObject colManagerObject; // 컬렉션 매니저 오브젝트 참조
    private GameObject slimeInfo; // 슬라임 정보 프리팹
    private Button button;

    public bool IsInfoOpen = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        colManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        colManager = colManagerObject.GetComponent<CollectionManager>();

        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();

        slimeInfo = Resources.Load<GameObject>(Paths.SlimeInfo);

        button.onClick.AddListener(SetSlimeInfo);
    }

    public void SetSlime(SlimeData slimeData)
    {
        SlimeId = slimeData.SlimeId;
        var iconData = DataTableManager.StringTable.Get(slimeData.SlimeIconId);
        var nameData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);

        Sprite iconSprite = Resources.Load<Sprite>(iconData.Value);
        slimeIcon.sprite = iconSprite;
        slimeNameText.text = nameData.Value;

        // 아이콘과 이름 표시 (수집된 슬라임)
        slimeIcon.gameObject.SetActive(true);
        slimeNameText.gameObject.SetActive(true);

        // 저장된 시간이 있으면 불러오고 없으면 현재 시간 저장
        if (SaveLoadManager.Data.CollectionTimes.TryGetValue(SlimeId, out string savedTime))
        {
            collectionTime = DateTime.Parse(savedTime);
        }
        else
        {
            collectionTime = DateTime.Now;
            SaveLoadManager.Data.CollectionTimes[SlimeId] = collectionTime.ToString("o");
            SaveLoadManager.Save();
        }
    }

    // 수집되지 않은 슬라임
    public void SetUnknownSlime(SlimeData slimeData)
    {
        SlimeId = slimeData.SlimeId;
        
        // 물음표 스프라이트 로드
        Sprite questionSprite = Resources.Load<Sprite>("QuestionMark");
        if (questionSprite != null)
        {
            slimeIcon.sprite = questionSprite;
        }
        else
        {
            Debug.LogWarning("QuestionMark 스프라이트를 찾을 수 없습니다. Resources 폴더에 QuestionMark.png 파일이 있는지 확인하세요.");
            // 기본 빈 스프라이트 사용
            slimeIcon.sprite = null;
        }
        
        slimeNameText.text = "???";
    }

    public void SetSlimeInfo()
    {
        if (SlimeId == 0)
        {
            return;
        }
        var slimeInfoGo = Instantiate(slimeInfo, uiManager.transform);
        collectionManager.IsInfoOpen = true;

        collectionPanel = GameObject.FindWithTag(Tags.CollectionPanel);
        collectionPanel.SetActive(false);

        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        var InfoData = DataTableManager.StringTable.Get(slimeData.SlimeInformationId);
        var StoryData = DataTableManager.StringTable.Get(slimeData.SlimeStoryId);

        slimeInfoGo.GetComponent<SlimeInfo>().slimeNameText.text = slimeNameText.text;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeDescriptionText.text = InfoData.Value;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeStoryText.text = StoryData.Value;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeImage.sprite = slimeIcon.sprite;
        slimeInfoGo.GetComponent<SlimeInfo>().slimeId = SlimeId;
        Debug.Log($"추가된 시간 {collectionTime}");
    }

    // 슬라임의 희귀도 반환
    public int GetRarity()
    {
        if (SlimeId == 0) return 0;
        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        return slimeData?.RarityId ?? 0;
    }

    // 슬라임의 이름 반환
    public string GetSlimeName()
    {
        if (SlimeId == 0)
        {
            return "";
        }
        return slimeNameText.text;
    }

    // 슬라임의 타입 반환
    public int GetSlimeType()
    {
        if (SlimeId == 0)
        {
            return 0;
        }

        var slimeData = DataTableManager.SlimeTable.Get(SlimeId);
        return slimeData?.SlimeTypeId ?? 0;
    }

    // 수집된 시간 반환
    public DateTime GetCollectionTime()
    {
        return collectionTime;
    }

    // 슬롯이 비어있는지 확인
    public bool IsEmpty()
    {
        return SlimeId == 0;
    }
    
    public void ClearSlot()
    {
        SlimeId = 0;
        
        // 아이콘과 이름 숨기기
        if (slimeIcon != null)
        {
            slimeIcon.sprite = null;
            slimeIcon.gameObject.SetActive(false);
        }
        
        // 텍스트 숨기기  
        if (slimeNameText != null)
        {
            slimeNameText.text = "";
            slimeNameText.gameObject.SetActive(false);
        }
    }
}
    
    

