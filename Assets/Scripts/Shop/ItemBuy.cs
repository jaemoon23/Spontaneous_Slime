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

    private InteriorData interiorData;

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

        // 초기 아이템 데이터 로드
        itemNameText.text = "아이템 이름"; // 실제 아이템 이름으로 설정
        itemDescriptionText.text = "아이템 설명"; // 실제 아이템 설명으로 설정
        itemPrice = 100; // 실제 아이템 가격으로 설정
        maxItemCount = 99; // 실제 최대 구매 가능 수량으로 설정
        totalPriceText.text = $"총 가격: {totalPrice} 골드";    // 초기 총 가격 설정
        warningText.text = string.Empty;    // 초기 경고 메시지 비움

        UpdateUI();
    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
    private void OnBuyButtonClicked()
    {
        
        if (CurrencyManager.Instance.CanAfford(totalPrice))
        {
            if (CurrencyManager.Instance.RemoveGold(totalPrice))
            {

                warningText.text = $"구매 완료: {itemNameText.text} x{itemCount} (총 {totalPrice} 골드)";
                // 아이템 지급 로직 (스위치로 아이템 가구인지 소모품인지 구별 후 지급)
                invenManager.AddConsumableItem(interiorData, itemCount);
                
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
        // itemNameText.text = itemData.Name;
        // itemDescriptionText.text = itemData.Description;
        // itemPrice = itemData.Price;
        // maxItemCount = itemData.MaxPurchaseCount;
        // itemCount = 1; // 구매 수량 초기화
        UpdateUI();
    }
}
