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
        //iconImage.sprite = Resources.Load<Sprite>(interiorData.InteriorIcon);
        // 인테리어 데이터를 슬롯에 설정하는 로직 구현
        furnitureItemUsePanel = Panel.GetComponent<FurnitureItemUsePanel>();
        this.count = count;
    }
    public void SetItem(ItemData itemData, int count)
    {
        this.itemData = itemData;
        // 아이템 데이터를 슬롯에 설정하는 로직 구현
        consumableItemUsePanel = Panel.GetComponent<ConsumableItemUsePanel>();
        iconImage.sprite = Resources.Load<Sprite>(itemData.ItemIcon);
        
        this.count = count;
    }
    
}