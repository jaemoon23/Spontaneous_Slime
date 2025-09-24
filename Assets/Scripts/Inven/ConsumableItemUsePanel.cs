using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemUsePanel : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button minButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button consumableCloseButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private TextMeshProUGUI quantityOwnedText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI warningText;

    private int itemCount = 0;

    private int quantityOwned = 50;  // 소유한 아이템 수량

    private void Start()
    {
        useButton.onClick.AddListener(OnUseButtonClick);
        minButton.onClick.AddListener(OnMinButtonClick);
        maxButton.onClick.AddListener(OnMaxButtonClick);
        plusButton.onClick.AddListener(OnPlusButtonClick);
        minusButton.onClick.AddListener(OnMinusButtonClick);
        consumableCloseButton.onClick.AddListener(OnconsumableCloseButtonCloseButtonClick);

        UpdateQuantityText();
        warningText.text = string.Empty;
    }
    private void OnconsumableCloseButtonCloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    private void OnUseButtonClick() // 아이템 사용 버튼 클릭 시
    {
        if (quantityOwned < itemCount)
        {
            warningText.text = "아이템이 부족합니다.";
            return;
        }
        else if (itemCount == 0)
        {
            warningText.text = "사용 수량을 선택해주세요.";
            return;
        }
        else
        {
            quantityOwned -= itemCount;
            // TODO: 아이템 사용 로직
            warningText.text = "아이템 사용 완료";
            UpdateQuantityText();
        }

    }
    private void OnMinButtonClick()
    {
        itemCount = 1;
        UpdateQuantityText();
    }

    private void OnMaxButtonClick()
    {
        itemCount = 99;
        UpdateQuantityText();
    }

    private void OnPlusButtonClick()
    {
        itemCount++;
        UpdateQuantityText();
    }

    private void OnMinusButtonClick()
    {
        itemCount--;
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (itemCount < 0)
        {
            itemCount = 0;
            warningText.text = "최소 수량은 0입니다.";
        }
        else if (itemCount > 99)
        {
            itemCount = 99;
            warningText.text = "최대 수량은 99입니다.";
        }
        itemCountText.text = itemCount.ToString();

        quantityOwnedText.text = $"보유 수량: {quantityOwned}";
    }

    public void SetItemUsePanel(ItemData itemData, int count)
    {
        var itemName = DataTableManager.StringTable.Get(itemData.ItemName);
        var itemDescription = DataTableManager.StringTable.Get(itemData.Description);
        // if (itemName == null || itemDescription == null)
        // {
        //     Debug.LogError($"아이템 데이터에 잘못된 참조가 있습니다: {itemData.ItemId}");
        //     return;
        // }
        // itemNameText.text = itemName.Value;
        // itemDescriptionText.text = itemDescription.Value;
        itemNameText.text = itemData.ItemName;
        itemDescriptionText.text = itemData.Description;
        quantityOwned = count;
        itemCount = 0; // 사용 수량 초기화
        UpdateQuantityText();
        warningText.text = string.Empty; // 경고 메시지 초기화
    }

}
    
