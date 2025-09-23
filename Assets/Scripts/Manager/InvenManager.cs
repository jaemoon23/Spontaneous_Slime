using System.Collections.Generic;
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
            // var itemData = DataTableManager.ItemTable.Get($"ITEM_{i+12}");
            // furnitureSlot.SetItem(itemData, 5); // 예시로 각 슬롯에
            furnitureSlot.Panel = furnitureItemUsePanel;
            furnitureInvenSlots.Add(furnitureSlot);
        }
        for (int i = 0; i < 12; i++)
        {
            GameObject slot = Instantiate(invenSlotPrefab, invenContentConsumableTransform);
            slot.name = slotNamePrefix + i;
            var consumableSlot = slot.GetComponent<InvenSlot>();

            // var itemData = DataTableManager.ItemTable.Get($"ITEM_{i}");
            // consumableSlot.SetItem(itemData, 5); // 예시로 각 슬롯에 5개 아이템 설정
            consumableSlot.Panel = consumableItemUsePanel;
            consumableInvenSlots.Add(consumableSlot);
            slot.SetActive(true);
        }
    }

    // 인벤토리 가구 아이템 추가 메서드
    public void AddFurnitureItem(ItemData itemData, int count)
    {
        if (furnitureInvenIndex < furnitureInvenSlots.Count)
        {
            furnitureInvenSlots[furnitureInvenIndex].SetItem(itemData, count);
            furnitureInvenIndex++;
        }
        else
        {
            Debug.Log("No more space in furniture inventory!");
        }
    }

    // 인벤토리 소모품 아이템 추가 메서드
    public void AddConsumableItem(ItemData itemData, int count)
    {
        if (consumableInvenIndex < consumableInvenSlots.Count)
        {
            consumableInvenSlots[consumableInvenIndex].SetItem(itemData, count);
            consumableInvenIndex++;
        }
        else
        {
            Debug.Log("No more space in consumable inventory!");
        }
    }

    // 버튼 클릭 이벤트 핸들러
    private void openButtonClick()
    {
        invenGameObject.SetActive(true);
    }

    private void closeButtonClick()
    {
        invenGameObject.SetActive(false);
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
}
