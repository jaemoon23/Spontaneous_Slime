using Excellcube.EasyTutorial.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

    [Header("References")]
    [SerializeField] private GameObject invenPanel;
    [SerializeField] private SlimeManager slimeManager;



    private int itemCount = 1;
    private int quantityOwned = 50;  // 소유한 아이템 수량
    private ItemData currentItemData;  // 현재 선택된 아이템 데이터
    private InvenManager invenManager;  // 인벤토리 매니저 참조

    private void Start()
    {
        useButton.onClick.AddListener(OnUseButtonClick);
        minButton.onClick.AddListener(OnMinButtonClick);
        maxButton.onClick.AddListener(OnMaxButtonClick);
        plusButton.onClick.AddListener(OnPlusButtonClick);
        minusButton.onClick.AddListener(OnMinusButtonClick);
        consumableCloseButton.onClick.AddListener(OnconsumableCloseButtonCloseButtonClick);

        // InvenManager 찾기
        var invenManagerObj = GameObject.FindWithTag(Tags.InvenManager);
        if (invenManagerObj != null)
        {
            invenManager = invenManagerObj.GetComponent<InvenManager>();
        }
        
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

        if (currentItemData.ItemId == 10105)
        {
            warningText.text = "도감에서 사용하세요";
            return;
        }

        // 츄르 아이템(ID: 2104)은 고양이 슬라임만 사용 가능
        if (currentItemData.ItemId == 2104)
        {
            var slimeManagerObj = GameObject.FindWithTag(Tags.SlimeManager);
            if (slimeManagerObj != null)
            {
                var slimeManager = slimeManagerObj.GetComponent<SlimeManager>();
                if (slimeManager != null && slimeManager.HasCurrentSlime())
                {
                    // 현재 슬라임이 고양이 슬라임이 아니면 사용 불가
                    if (slimeManager.GetCurrentSlimeType() != SlimeType.Cat)
                    {
                        warningText.text = "츄르는 고양이 슬라임만 사용할 수 있습니다!";
                        Debug.Log("츄르 사용 실패: 고양이 슬라임이 아님");
                        return; // 여기서 완전히 중단
                    }
                }
                else
                {
                    warningText.text = "현재 슬라임이 없습니다!";
                    return; // 여기서 완전히 중단
                }
            }
            else
            {
                warningText.text = "SlimeManager를 찾을 수 없습니다!";
                return; // 여기서 완전히 중단
            }

        }

        // 인벤토리에서 아이템 제거 시도
        if (invenManager != null && invenManager.RemoveConsumableItem(currentItemData, itemCount))
        {
            quantityOwned -= itemCount;

            // 아이템 사용 로직 구현
            UseItem(currentItemData, itemCount);

            warningText.text = "아이템 사용 완료";
            UpdateQuantityText();
            gameObject.SetActive(false);
            invenPanel.SetActive(false);
        }
        else
        {
            warningText.text = "아이템 사용에 실패했습니다.";
        }
        if (PlayerPrefs.GetInt("ECET_CLEAR_ALL") == 0)
        {
            TutorialEvent.Instance.Broadcast("TUTORIAL_PRESSED_USE");
        }
        
    }
    private void OnMinButtonClick()
    {
        itemCount = 1;
        UpdateQuantityText();
    }

    private void OnMaxButtonClick()
    {
        var slimeData = DataTableManager.SlimeTable.Get(slimeManager.CurrentSlimeId);
        var ItemOptionValue = currentItemData.ItemOptionValue;
        var exp = slimeManager.GetCurrentSlime().GetComponent<SlimeGrowth>().cumulativeExp;
        
        Debug.Log(exp);
        if (quantityOwned > (slimeData.MaxExp - exp) / ItemOptionValue + 1) 
        {
            itemCount = (slimeData.MaxExp - exp) / ItemOptionValue + 1; // 최대 사용 가능 수량 계산
        }
        
        UpdateQuantityText();
    }

    private void OnPlusButtonClick()
    {
        itemCount++;
        UpdateQuantityText();
    }

    private void OnMinusButtonClick()
    {
        if (itemCount <= 1)
        {
            return;
        }
        itemCount--;
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (itemCount < 1)
        {
            itemCount = 1;
            warningText.text = "최소 수량은 1입니다.";
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
        currentItemData = itemData;  // 현재 아이템 데이터 저장
        
        var itemName = DataTableManager.StringTable.Get(itemData.ItemName);
        var itemDescription = DataTableManager.StringTable.Get(itemData.Description);
        if (itemName == null || itemDescription == null)
        {
            Debug.LogError($"아이템 데이터에 잘못된 참조가 있습니다: {itemData.ItemId}");
            return;
        }
        itemNameText.text = itemName.Value;
        itemDescriptionText.text = itemDescription.Value;
        quantityOwned = count;
        itemCount = 0; // 사용 수량 초기화
        UpdateQuantityText();
        warningText.text = string.Empty; // 경고 메시지 초기화
    }

    private void UseItem(ItemData itemData, int useCount)
    {
        if (itemData == null)
        {
            Debug.LogError("아이템 데이터가 없습니다!");
            return;
        }

        // OPTION_TYPE이 1인 경우: 경험치 아이템
        if (itemData.ItemOptionType == 1)
        {
            // SlimeManager를 통해 현재 슬라임 찾기
            var slimeManagerObj = GameObject.FindWithTag(Tags.SlimeManager);
            if (slimeManagerObj != null)
            {
                var slimeManager = slimeManagerObj.GetComponent<SlimeManager>();
                if (slimeManager != null && slimeManager.HasCurrentSlime())
                {
                    var currentSlime = slimeManager.GetCurrentSlime();
                    var slimeGrowth = currentSlime.GetComponent<SlimeGrowth>();
                    
                    if (slimeGrowth != null)
                    {
                        int totalExp = itemData.ItemOptionValue * useCount;
                        slimeGrowth.AddExp(totalExp);
                        Debug.Log($"경험치 아이템 사용: {itemData.ItemName} x{useCount}, 총 경험치 +{totalExp}");
                    }
                    else
                    {
                        Debug.LogError("현재 슬라임에서 SlimeGrowth 컴포넌트를 찾을 수 없습니다!");
                    }
                }
                else
                {
                    Debug.LogError("현재 슬라임이 없습니다!");
                }
            }
            else
            {
                Debug.LogError("SlimeManager를 찾을 수 없습니다!");
            }
        }
        else
        {
            Debug.LogWarning($"지원되지 않는 아이템 타입입니다: OPTION_TYPE = {itemData.ItemOptionType}");
        }
    }

}
    
