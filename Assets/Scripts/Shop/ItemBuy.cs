using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuy : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Button minButton;
    [SerializeField] private Button maxButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button closeButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private TextMeshProUGUI totalPriceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("References")]
    [SerializeField] private InvenManager invenManager;


    private StoreData storeData;
    private int itemCount = 1;
    private int itemPrice = 100; // 예시 가격, 실제로는 아이템 데이터에서 가져와야 함
    private int totalPrice => itemCount * itemPrice;
    private int maxItemCount = 99; // 최대 구매 가능 수량 (아이템 데이터에서 가져올 수 있음)

    private void Start()
    {
        // 버튼 클릭 이벤트 등록
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        minButton.onClick.AddListener(OnMinButtonClicked);
        maxButton.onClick.AddListener(OnMaxButtonClicked);
        plusButton.onClick.AddListener(OnPlusButtonClicked);
        minusButton.onClick.AddListener(OnMinusButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        UpdateUI();
    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnBuyButtonClicked()
    {
        if (invenManager == null)
        {
            warningText.text = "인벤토리 시스템이 연결되지 않았습니다!";
            return;
        }

        if (CurrencyManager.Instance.CanAfford(totalPrice))
        {
            if (CurrencyManager.Instance.RemoveGold(totalPrice))
            {
                warningText.text = $"구매 완료: {itemNameText.text} x{itemCount} (총 {totalPrice} 골드)";
                
                // 아이템 지급 로직
                switch (storeData.productType)
                {
                    case 10: // 인테리어
                        var interiorData = DataTableManager.InteriorTable.Get(storeData.productId);
                        if (interiorData != null)
                        {
                            invenManager.AddInteriorItem(interiorData, itemCount);
                            Debug.Log($"인테리어 아이템 추가: {interiorData} x{itemCount}");
                        }
                        else
                        {
                            Debug.LogError($"인테리어 데이터를 찾을 수 없음: {storeData.productId}");
                        }
                        break;
                        
                    case 2: // 소모품
                        var itemData = DataTableManager.ItemTable.Get(storeData.productId);
                        if (itemData != null)
                        {
                            invenManager.AddConsumableItem(itemData, itemCount);
                            Debug.Log($"소모품 아이템 추가: {itemData} x{itemCount}");
                        }
                        else
                        {
                            Debug.LogError($"아이템 데이터를 찾을 수 없음: {storeData.productId}");
                        }
                        break;

                    default:
                        Debug.LogWarning($"알 수 없는 아이템 타입: {storeData.productType}");
                        break;
                }
            }
        }
        else
        {
            warningText.text = "골드가 부족합니다!";
        }
    }
    private void OnMinButtonClicked()
    {
        itemCount = 1;
        UpdateUI();
    }
    private void OnMaxButtonClicked()
    {
        itemCount = maxItemCount;
        UpdateUI();
    }
    private void OnPlusButtonClicked()
    {
        if (itemCount < maxItemCount)
        {
            itemCount++;
            UpdateUI();
        }
    }
    private void OnMinusButtonClicked()
    {
        if (itemCount > 1)
        {
            itemCount--;
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        itemCountText.text = itemCount.ToString();
        totalPriceText.text = $"총 가격: {totalPrice} 골드";
    }

    public void SetItemBuy(InteriorData interiorData)
    {
        // var  = DataTableManager.StringTable.Get(interiorData.);

        // itemNameText.text = "아이템 이름"; // 실제 아이템 이름으로 설정
        // itemDescriptionText.text = "아이템 설명"; // 실제 아이템 설명으로 설정
        // itemPrice = 100; // 실제 아이템 가격으로 설정
        // maxItemCount = 99; // 실제 최대 구매 가능 수량으로 설정
        // totalPriceText.text = $"총 가격: {totalPrice} 골드";    // 초기 총 가격 설정
        // warningText.text = string.Empty;    // 초기 경고 메시지 비움
        UpdateUI();
    }
    public void SetItemBuy(ItemData itemData)
    {
        // itemNameText.text = itemData.Name;
        // itemDescriptionText.text = itemData.Description;
        // itemPrice = itemData.Price;
        // maxItemCount = itemData.MaxPurchaseCount;
        // itemCount = 1; // 구매 수량 초기화
        UpdateUI();
    }
    public void SetItemBuy(StoreData storeData)
    {
        this.storeData = storeData;
        var name = DataTableManager.StringTable.Get(storeData.productName);
        var description = DataTableManager.StringTable.Get(storeData.productDescription);
        itemNameText.text = name.Value; // 아이템 이름
        itemDescriptionText.text = description.Value; // 아이템 설명
        itemPrice = storeData.price; // 아이템 가격
        maxItemCount = storeData.oneTimeMaxQty; // 최대 구매 가능 수량
        totalPriceText.text = $"총 가격: {totalPrice} 골드";
        warningText.text = string.Empty;
        UpdateUI(); 
    }
}
