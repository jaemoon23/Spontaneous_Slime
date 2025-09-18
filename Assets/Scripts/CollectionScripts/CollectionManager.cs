using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;

public class CollectionManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button collectionButtonClose;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private GameObject collectionUI;
    [SerializeField] private GameObject[] slimeCollectionPanels;    // 도감 패널

    [SerializeField] private List<CollectionSlot> slots = new List<CollectionSlot>();
    public List<SlimeData> slimeDatas = new List<SlimeData>();

    [SerializeField] private GameObject infoPanel; // 슬라임 정보 패널
    [SerializeField] private GameObject Slider;
    [SerializeField] private GameObject levelText;

    public Button levelUpButton; // 레벨업 버튼
    public Button environmentButton; // 먹기 버튼

    public bool IsInfoOpen { get; set; } = false;

    private int slotIndex = 0;
    private int pageIndex = 0;

    void Start()
    {
        pageIndex = 0;
        // 도감 활성 비활성
        collectionButton.onClick.AddListener(OpenCollectionUI);
        collectionButtonClose.onClick.AddListener(CloseCollectionUI);


        // 페이지 넘기기
        leftArrowButton.onClick.AddListener(OnClickLeftArrow);
        rightArrowButton.onClick.AddListener(OnClickRightArrow);

        // 저장된 도감 데이터 로드
        LoadCollectionData();
        collectionButton.interactable = true;
    }


    // 도감 UI 열기
    public void OpenCollectionUI()
    {
        collectionUI.SetActive(true);
        collectionButton.gameObject.SetActive(false);
        Slider.SetActive(false);
        levelText.SetActive(false);
        levelUpButton.gameObject.SetActive(false);
        environmentButton.gameObject.SetActive(false);
        SaveCollectionData(); // UI 상태 변경 저장
    }


    // 도감 UI 닫기
    public void CloseCollectionUI()
    {
        collectionUI.SetActive(false);
        collectionButton.gameObject.SetActive(true);
        Slider.SetActive(true);
        levelText.SetActive(true);
        levelUpButton.gameObject.SetActive(true);
        environmentButton.gameObject.SetActive(true);
        SaveCollectionData(); // UI 상태 변경 저장
    }

    // 왼쪽 화살표 클릭 (이전 페이지)
    public void OnClickLeftArrow()
    {

        if (pageIndex > 0)
        {
            int temp = pageIndex;
            pageIndex--;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[pageIndex].SetActive(true);
            
            // 페이지 변경사항 저장
            SaveCollectionData();
        }
    }

    // 오른쪽 화살표 클릭 (다음 페이지)
    public void OnClickRightArrow()
    {
        if (pageIndex < slimeCollectionPanels.Length - 1)
        {
            int temp = pageIndex;
            pageIndex++;
            slimeCollectionPanels[temp].SetActive(false);
            slimeCollectionPanels[pageIndex].SetActive(true);
            
            // 페이지 변경사항 저장
            SaveCollectionData();
        }
    }

    public void AddCollection(SlimeData slimeData)
    {
        if (slotIndex >= slots.Count)
        {
            Debug.LogWarning("더 이상 도감 슬롯이 없습니다.");
            return;
        }

        if (slots.Exists(slot => slot.SlimeId == slimeData.SlimeId))
        {
            Debug.LogWarning("이미 도감에 추가된 슬라임입니다.");
            return;
        }
        slots[slotIndex].SetSlime(slimeData);
        slotIndex++;
        
        // 도감 데이터를 SaveData에 저장
        SaveCollectionData();
    }
    
    // 도감 데이터를 SaveData에 저장
    public void SaveCollectionData()
    {
        var saveData = SaveLoadManager.Data;
        
        // 수집된 슬라임 ID 목록 업데이트
        saveData.CollectedSlimeIds.Clear();
        foreach (var slot in slots)
        {
            if (slot.SlimeId != 0) // 슬라임이 설정된 슬롯만
            {
                saveData.CollectedSlimeIds.Add(slot.SlimeId);
            }
        }
        
        // 인덱스 정보 저장
        saveData.CollectionSlotIndex = slotIndex;
        saveData.CollectionPageIndex = pageIndex;
        
        // UI 상태 저장
        saveData.IsCollectionUIOpen = collectionUI.activeSelf;
        saveData.IsInfoPanelOpen = IsInfoOpen;
        
        Debug.Log($"도감 데이터 저장됨: 수집된 슬라임 {saveData.CollectedSlimeIds.Count}개");
    }
    
    // SaveData에서 도감 데이터 로드
    public void LoadCollectionData()
    {
        var saveData = SaveLoadManager.Data;
        
        Debug.Log($"도감 데이터 로드 시작: 저장된 슬라임 {saveData.CollectedSlimeIds.Count}개, 슬롯인덱스: {saveData.CollectionSlotIndex}, 페이지인덱스: {saveData.CollectionPageIndex}");
        
        // 슬롯 인덱스 복원
        slotIndex = saveData.CollectionSlotIndex;
        pageIndex = saveData.CollectionPageIndex;
        
        // 수집된 슬라임들을 슬롯에 설정
        for (int i = 0; i < saveData.CollectedSlimeIds.Count && i < slots.Count; i++)
        {
            var slimeData = DataTableManager.SlimeTable.Get(saveData.CollectedSlimeIds[i]);
            if (slimeData != null)
            {
                Debug.Log($"슬라임 {saveData.CollectedSlimeIds[i]} 슬롯 {i}에 복원");
                slots[i].SetSlime(slimeData);
            }
            else
            {
                Debug.LogWarning($"슬라임 ID {saveData.CollectedSlimeIds[i]}에 대한 데이터를 찾을 수 없습니다.");
            }
        }
        
        // UI 상태 복원
        collectionUI.SetActive(saveData.IsCollectionUIOpen);
        collectionButton.gameObject.SetActive(!saveData.IsCollectionUIOpen);
        IsInfoOpen = saveData.IsInfoPanelOpen;
        
        // 페이지 설정
        if (pageIndex < slimeCollectionPanels.Length)
        {
            for (int i = 0; i < slimeCollectionPanels.Length; i++)
            {
                slimeCollectionPanels[i].SetActive(i == pageIndex);
            }
        }
        
        Debug.Log($"도감 데이터 로드 완료: 수집된 슬라임 {saveData.CollectedSlimeIds.Count}개");
    }
    
    public void OpenSlimeCollection()
    {
        collectionUI.SetActive(true);
    }

}