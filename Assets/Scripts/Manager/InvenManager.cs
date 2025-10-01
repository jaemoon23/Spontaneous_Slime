using System.Collections.Generic;
using Excellcube.EasyTutorial.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvenManager : MonoBehaviour
{
    private readonly string slotNamePrefix = "InvenSlot_";

    // InvenSlot 프리팹, 컨텐츠 트랜스폼
    private GameObject invenSlotPrefab;
    [SerializeField] private Transform invenContentFurnitureTransform;
    [SerializeField] private Transform invenContentConsumableTransform;

    [Header("Inven GameObjects")]
    [SerializeField] private GameObject invenContentConsumableGameObject;
    [SerializeField] private GameObject invenContentFurnitureGameObject;
    [SerializeField] private GameObject invenGameObject;

    [Header("Inven Tab Buttons")]
    [SerializeField] private Button FurnitureButton;
    [SerializeField] private Button ConsumableButton;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button openButton;

    [SerializeField] private GameObject[] furnitureInvenSlotArray;
    [SerializeField] private GameObject[] consumableInvenSlotArray;
    // 인벤토리 슬롯 리스트, 인덱스
    private List<InvenSlot> furnitureInvenSlots = new List<InvenSlot>();
    private List<InvenSlot> consumableInvenSlots = new List<InvenSlot>();

    private int furnitureInvenIndex = 0;
    private int consumableInvenIndex = 0;

    [SerializeField] private GameObject consumableItemUsePanel;
    [SerializeField] private GameObject furnitureItemUsePanel;
    private void Start()
    {
        // 버튼 클릭 이벤트 리스너 등록
        openButton.onClick.AddListener(openButtonClick);
        closeButton.onClick.AddListener(closeButtonClick);
        FurnitureButton.onClick.AddListener(FurnitureButtonClick);
        ConsumableButton.onClick.AddListener(ConsumableButtonClick);

        // 인벤토리 슬롯 생성
        invenSlotPrefab = Resources.Load<GameObject>(Paths.InvenSlot);

        for (int i = 0; i < 12; i++)
        {
            GameObject slot = Instantiate(invenSlotPrefab, invenContentFurnitureTransform);
            slot.name = slotNamePrefix + i;
            var furnitureSlot = slot.GetComponent<InvenSlot>();

            furnitureSlot.Panel = furnitureItemUsePanel;
            furnitureInvenSlots.Add(furnitureSlot);
            slot.SetActive(false);
        }
        for (int i = 0; i < 12; i++)
        {
            GameObject slot = Instantiate(invenSlotPrefab, invenContentConsumableTransform);
            slot.name = slotNamePrefix + i;
            var consumableSlot = slot.GetComponent<InvenSlot>();


            consumableSlot.Panel = consumableItemUsePanel;
            consumableInvenSlots.Add(consumableSlot);
            slot.SetActive(false);
        }
    }

    // 인벤토리 소모품 아이템 추가 메서드 (99개까지 합산, 초과시 새 슬롯 생성)
    public void AddConsumableItem(ItemData itemData, int count)
    {
        int remainingCount = count;
        
        // 기존 슬롯에서 같은 아이템 찾아서 99개까지 합산
        for (int i = 0; i < consumableInvenSlots.Count && remainingCount > 0; i++)
        {
            var slot = consumableInvenSlots[i];
            if (slot.gameObject.activeSelf && slot.GetItemData() != null && 
                slot.GetItemData().ItemId == itemData.ItemId)
            {
                int currentCount = slot.GetItemCount();
                int canAdd = Mathf.Min(99 - currentCount, remainingCount);
                
                if (canAdd > 0)
                {
                    slot.SetItem(itemData, currentCount + canAdd);
                    remainingCount -= canAdd;
                    Debug.Log($"기존 슬롯 {i}에 {canAdd}개 추가. 현재: {currentCount + canAdd}개");
                }
            }
        }
        
        // 남은 수량이 있으면 새 슬롯에 추가
        while (remainingCount > 0 && consumableInvenIndex < consumableInvenSlots.Count)
        {
            int addCount = Mathf.Min(99, remainingCount);
            consumableInvenSlots[consumableInvenIndex].SetItem(itemData, addCount);
            consumableInvenIndex++;
            remainingCount -= addCount;
            Debug.Log($"새 슬롯 {consumableInvenIndex - 1}에 {addCount}개 추가");
        }
        
        if (remainingCount > 0)
        {
            Debug.LogWarning($"소모품 인벤토리에 공간이 부족합니다! {remainingCount}개 추가 실패");
        }
    }
    // 인벤토리 인테리어 아이템 추가 메서드
    public void AddInteriorItem(InteriorData interiorData, int count)
    {
        if (furnitureInvenIndex < furnitureInvenSlots.Count)
        {
            furnitureInvenSlots[furnitureInvenIndex].SetItem(interiorData, count);
            furnitureInvenIndex++;
        }
        else
        {
            Debug.Log("가구 인벤토리에 더 이상 공간이 없습니다!");
        }
    }

    // 인벤토리 소모품 아이템 제거 메서드
    public bool RemoveConsumableItem(ItemData itemData, int count)
    {
        for (int i = 0; i < consumableInvenSlots.Count; i++)
        {
            var slot = consumableInvenSlots[i];
            if (slot.GetItemData() != null && slot.GetItemData().ItemId == itemData.ItemId)
            {
                int currentCount = slot.GetItemCount();
                if (currentCount >= count)
                {
                    int newCount = currentCount - count;
                    if (newCount > 0)
                    {
                        slot.SetItem(itemData, newCount);
                    }
                    else
                    {
                        slot.ClearItem();
                        // 슬롯이 비워지면 인덱스는 그대로 유지 (빈 슬롯으로 남겨둠)
                    }
                    return true;
                }
                else
                {
                    Debug.LogWarning($"아이템 수량이 부족합니다. 요청: {count}, 보유: {currentCount}");
                    return false;
                }
            }
        }
        Debug.LogWarning($"해당 아이템을 찾을 수 없습니다: {itemData.ItemId}");
        return false;
    }

    // 인벤토리 데이터 저장 (슬롯별 저장)
    public void SaveInventoryData()
    {
        var saveData = SaveLoadManager.Data;

        // 소비성 아이템 슬롯별 저장
        saveData.ConsumableSlots.Clear();
        for (int i = 0; i < consumableInvenSlots.Count; i++)
        {
            var slot = consumableInvenSlots[i];
            var itemData = slot.GetItemData();
            if (itemData != null && slot.GetItemCount() > 0)
            {
                saveData.ConsumableSlots.Add(new SaveDataV1.InventorySlot
                {
                    ItemId = itemData.ItemId,
                    Count = slot.GetItemCount(),
                    SlotIndex = i
                });
            }
        }

        // 가구 아이템 저장 (기존 방식 유지)
        saveData.FurnitureItems.Clear();
        foreach (var slot in furnitureInvenSlots)
        {
            var interiorData = slot.GetInteriorData();
            if (interiorData != null && slot.GetItemCount() > 0)
            {
                saveData.FurnitureItems[interiorData.InteriorId] = slot.GetItemCount();
            }
        }

        Debug.Log($"인벤토리 데이터 저장 완료: 소비품 슬롯 {saveData.ConsumableSlots.Count}개, 가구 {saveData.FurnitureItems.Count}개");
    }

    // 인벤토리 데이터 로드 (슬롯별 로드)
    public void LoadInventoryData()
    {
        var saveData = SaveLoadManager.Data;

        // 기존 인벤토리 초기화
        ClearAllSlots();
        consumableInvenIndex = 0;
        furnitureInvenIndex = 0;

        // 소비성 아이템 슬롯별 로드
        foreach (var slotData in saveData.ConsumableSlots)
        {
            var itemData = DataTableManager.ItemTable.Get(slotData.ItemId);
            if (itemData != null && slotData.SlotIndex < consumableInvenSlots.Count)
            {
                consumableInvenSlots[slotData.SlotIndex].SetItem(itemData, slotData.Count);
                // 인덱스 업데이트 (가장 높은 슬롯 인덱스 + 1)
                if (slotData.SlotIndex >= consumableInvenIndex)
                {
                    consumableInvenIndex = slotData.SlotIndex + 1;
                }
            }
        }

        // 가구 아이템 로드 (기존 방식 유지)
        foreach (var kvp in saveData.FurnitureItems)
        {
            var interiorData = DataTableManager.InteriorTable.Get(kvp.Key);
            if (interiorData != null)
            {
                AddInteriorItem(interiorData, kvp.Value);
            }
        }

        Debug.Log($"인벤토리 데이터 로드 완료: 소비품 슬롯 {saveData.ConsumableSlots.Count}개, 가구 {saveData.FurnitureItems.Count}개");
    }

    // 모든 슬롯 초기화 메서드
    private void ClearAllSlots()
    {
        foreach (var slot in consumableInvenSlots)
        {
            slot.ClearItem();
        }
        foreach (var slot in furnitureInvenSlots)
        {
            slot.ClearItem();
        }
    }

    // 버튼 클릭 이벤트 핸들러
    private void openButtonClick()
    {
        invenGameObject.SetActive(true);
        UISoundManager.Instance.PlayOpenSound();
        if (PlayerPrefs.GetInt("ECET_CLEAR_ALL") == 0)
        {
            TutorialEvent.Instance.Broadcast("TUTORIAL_OPEN_INVEN");
        }
    }

    private void closeButtonClick()
    {
        invenGameObject.SetActive(false);
        UISoundManager.Instance.PlayCloseSound();
    }

    private void FurnitureButtonClick()
    {
        invenContentFurnitureGameObject.SetActive(true);
        invenContentConsumableGameObject.SetActive(false);
    }

    private void ConsumableButtonClick()
    {
        invenContentFurnitureGameObject.SetActive(false);
        invenContentConsumableGameObject.SetActive(true);
    }

    // 소모품 아이템 개수 조회 메서드
    public int GetConsumableItemCount(int itemId)
    {
        foreach (var slot in consumableInvenSlots)
        {
            var itemData = slot.GetItemData();
            if (itemData != null && itemData.ItemId == itemId)
            {
                return slot.GetItemCount();
            }
        }
        return 0; // 해당 아이템이 없으면 0 반환
    }

    public bool FindInterior(int interiorId)
    {
        foreach (var slot in furnitureInvenSlots)
        {
            var interiorData = slot.GetInteriorData();
            if (interiorData != null && interiorData.InteriorId == interiorId)
            {
                Debug.Log("인테리어 아이템 발견: " + interiorData.InteriorName);
                return true;
            }
        }
        Debug.Log("인테리어 아이템을 찾을 수 없습니다.");
        return false;
    }
}
