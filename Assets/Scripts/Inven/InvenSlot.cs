using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    private Button button;
    public GameObject Panel;
    private ConsumableItemUsePanel consumableItemUsePanel;
    private FurnitureItemUsePanel furnitureItemUsePanel;

    private ItemData itemData;
    private InteriorData interiorData;
    public int id { get; private set; }
    [SerializeField] private Image iconImage;
    private int count = 0;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (Panel == null)
        {
            Debug.LogWarning("Panel이 할당되지 않았습니다!");
            return;
        }
        if (consumableItemUsePanel == null && furnitureItemUsePanel == null)
        {
            Debug.LogWarning("ItemUsePanel이 할당되지 않았습니다!");
            return;
        }
        switch (Panel.name)
        {
            case "ConsumableItemUsePanel":
                consumableItemUsePanel.SetItemUsePanel(itemData, count);
                break;
            case "FurnitureItemUsePanel":
                furnitureItemUsePanel.SetInteriorUsePanel(interiorData, count);
                break;
            default:
                Debug.LogWarning($"알 수 없는 패널 이름: {Panel.name}");
                break;
        }
        
        Panel.SetActive(true);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
    public void SetItem(InteriorData interiorData, int count)
    {
        this.interiorData = interiorData;
        var iconString = DataTableManager.StringTable.Get(interiorData.Icon);
        iconImage.sprite = Resources.Load<Sprite>(iconString.Value);
        // 인테리어 데이터를 슬롯에 설정하는 로직 구현
        furnitureItemUsePanel = Panel.GetComponent<FurnitureItemUsePanel>();
        id = interiorData.InteriorId;
        this.count = count;
        gameObject.SetActive(true);
    }
    public void SetItem(ItemData itemData, int count)
    {
        this.itemData = itemData;
        // 아이템 데이터를 슬롯에 설정하는 로직 구현
        consumableItemUsePanel = Panel.GetComponent<ConsumableItemUsePanel>();
        var iconData = DataTableManager.StringTable.Get(itemData.ItemIcon);
        iconImage.sprite = Resources.Load<Sprite>(iconData.Value);
        id = itemData.ItemId;
        this.count = count;
        gameObject.SetActive(true);
    }
    
    // 아이템 데이터 반환
    public ItemData GetItemData()
    {
        return itemData;
    }
    
    // 인테리어 데이터 반환
    public InteriorData GetInteriorData()
    {
        return interiorData;
    }
    
    // 아이템 수량 반환
    public int GetItemCount()
    {
        return count;
    }

    // 슬롯 비우기
    public void ClearItem()
    {
        itemData = null;
        interiorData = null;
        count = 0;
        iconImage.sprite = null;
        gameObject.SetActive(false);
    }
}