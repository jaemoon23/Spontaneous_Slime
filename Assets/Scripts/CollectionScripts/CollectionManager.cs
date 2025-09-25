using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;
using System;

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

    public Button levelUpButton; // 레벨업 버튼
    public Button environmentButton; // 먹기 버튼

    [SerializeField] private Button AcquisitionSortButton;
    [SerializeField] private Button RaritySortButton;
    [SerializeField] private Button NameSortButton;

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

        // 정렬 버튼 이벤트 연결
        AcquisitionSortButton.onClick.AddListener(AcquisitionSort);
        RaritySortButton.onClick.AddListener(RaritySort);
        NameSortButton.onClick.AddListener(NameSort);

        // 저장된 도감 데이터 로드
        LoadCollectionData();
        collectionButton.interactable = true;
    }

    // 도감 UI 열기
    public void OpenCollectionUI()
    {
        collectionUI.SetActive(true);
        collectionButton.gameObject.SetActive(false);

        levelUpButton.gameObject.SetActive(false);
        environmentButton.gameObject.SetActive(false);
        SaveCollectionData(); // UI 상태 변경 저장
    }

    // 도감 UI 닫기
    public void CloseCollectionUI()
    {
        collectionUI.SetActive(false);
        collectionButton.gameObject.SetActive(true);

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

        // 수집 시간 저장
        SaveLoadManager.Data.CollectionTimes[slimeData.SlimeId] = System.DateTime.Now.ToString();

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
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < saveData.CollectedSlimeIds.Count && saveData.CollectedSlimeIds[i] != 0) 
            {
                var slimeData = DataTableManager.SlimeTable.Get(saveData.CollectedSlimeIds[i]);
                if (slimeData != null)
                {
                    Debug.Log($"슬라임 {saveData.CollectedSlimeIds[i]} 슬롯 {i}에 복원");
                    slots[i].SetSlime(slimeData);
                }
                else
                {
                    // 언락 아이콘 설정
                    slots[i].slimeIcon.sprite = Resources.Load<Sprite>("Icons/UNLOCK");
                    slots[i].slimeIcon.gameObject.SetActive(true);
                }
            }
            else  // 수집된 슬라임이 없거나 0인 슬롯
            {
                slots[i].slimeIcon.sprite = Resources.Load<Sprite>("Icons/UNLOCK");
                slots[i].slimeIcon.gameObject.SetActive(true);
            }
        }

        // UI 강제 업데이트
        Canvas.ForceUpdateCanvases();

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

    public void AcquisitionSort()
    {
        // 현재 설정된 SlimeData들을 임시 리스트에 저장
        List<SlimeData> slimeDataList = new List<SlimeData>();
        
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty())
            {
                slimeDataList.Add(DataTableManager.SlimeTable.Get(slot.SlimeId));
            }
        }
        
        // SlimeData를 획득 순으로 정렬 (수집 시간 기준)
        slimeDataList.Sort((a, b) => {
            var timeA = GetCollectionTimeForSlime(a.SlimeId);
            var timeB = GetCollectionTimeForSlime(b.SlimeId);
            return timeA.CompareTo(timeB);
        });
        
        // 모든 슬롯 초기화
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].ClearSlot();
        }
        
        // 정렬된 순서대로 슬롯에 재설정
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].SetSlime(slimeDataList[i]);
        }
        Canvas.ForceUpdateCanvases();
        
        // slotIndex 업데이트
        //slotIndex = slimeDataList.Count;
        
        SaveCollectionData();
    }
    public void RaritySort()
    {
        // 현재 설정된 SlimeData들을 임시 리스트에 저장
        List<SlimeData> slimeDataList = new List<SlimeData>();
        
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty())
            {
                slimeDataList.Add(DataTableManager.SlimeTable.Get(slot.SlimeId));
            }
        }
        
        // SlimeData를 희귀도 순으로 정렬
        slimeDataList.Sort((a, b) => a.RarityId.CompareTo(b.RarityId));
        
        // 슬롯 초기화
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].ClearSlot();
        }
        
        // 정렬된 순서대로 슬롯에 재설정
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].SetSlime(slimeDataList[i]);
        }
        Canvas.ForceUpdateCanvases();
        // slotIndex 업데이트
        //slotIndex = slimeDataList.Count;
        
        SaveCollectionData();
    }
    public void NameSort()
    {
        // 현재 설정된 SlimeData들을 임시 리스트에 저장
        List<SlimeData> slimeDataList = new List<SlimeData>();
        
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty())
            {
                slimeDataList.Add(DataTableManager.SlimeTable.Get(slot.SlimeId));
            }
        }

        // SlimeData를 이름 순으로 정렬
        slimeDataList.Sort((a, b) => a.SlimeName.CompareTo(b.SlimeName));
        
        // 모든 슬롯 초기화
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].ClearSlot();
        }
        
        // 정렬된 순서대로 슬롯에 재설정
        for (int i = 0; i < slotIndex; i++)
        {
            slots[i].SetSlime(slimeDataList[i]);
        } 
        Canvas.ForceUpdateCanvases();
        // slotIndex 업데이트
        //slotIndex = slimeDataList.Count;
        
        SaveCollectionData();
    }

    public void UpdateCollectionUI()
    {
        // 임시 리스트에 현재 슬롯들의 SlimeData 저장
        List<SlimeData> tempSlimeDataList = new List<SlimeData>();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].SlimeId != 0)
            {
                tempSlimeDataList.Add(DataTableManager.SlimeTable.Get(slots[i].SlimeId));
            }
            else
            {
                tempSlimeDataList.Add(null);
            }
        }

        // 모든 슬롯 초기화
        for (int i = 0; i < slotIndex; i++)  // ← slotIndex 만큼만 ClearSlot() 호출
        {
            slots[i].ClearSlot(); // 슬롯 비우기
        }

        // 정렬된 순서대로 슬롯에 재배치
        for (int i = 0; i < tempSlimeDataList.Count; i++)
        {
            if (tempSlimeDataList[i] != null)
            {
                slots[i].SetSlime(tempSlimeDataList[i]);
            }
        }
    }

    // 슬라임의 수집 시간 가져오기
    private DateTime GetCollectionTimeForSlime(int slimeId)
    {
        if (SaveLoadManager.Data.CollectionTimes.TryGetValue(slimeId, out string savedTime))
        {
            return DateTime.Parse(savedTime);
        }
        return DateTime.Now;
    }
    
    
}