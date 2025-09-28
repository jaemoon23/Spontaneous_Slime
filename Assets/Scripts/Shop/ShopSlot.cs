using UnityEngine;
using UnityEngine.UI;
using Excellcube.EasyTutorial.Utils;

public class ShopSlot : MonoBehaviour
{
    private Button button;
    private GameObject itemBuyPanel;
    private GameObject shopPanel;
    private StoreData storeData;
    private ItemBuy itemBuy;  
    // 아이템 데이터
    [SerializeField] private Image iconImage;

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
        
        if (itemBuyPanel != null)
        {
            if (itemBuy == null)
            {
                itemBuy = itemBuyPanel.GetComponent<ItemBuy>();
                Debug.LogWarning("ItemBuy 컴포넌트가 할당되지 않았습니다.");
            }
            if (itemBuy != null)
            {
                itemBuy.SetItemBuy(storeData);
                itemBuyPanel.SetActive(true);
                shopPanel.SetActive(false);
                if (PlayerPrefs.GetInt("ECET_CLEAR_ALL") == 0)
                {
                    TutorialEvent.Instance.Broadcast("TUTORIAL_BUTTON_SHOP_BUY");
                }
                // switch (storeData.productType)
                // {
                //     case 10: // 인테리어
                //         var interiorData = DataTableManager.InteriorTable.Get(storeData.productId);
                //         itemBuy.SetItemBuy(interiorData);
                //         break;
                //     case 2: // 아이템
                //         var itemData = DataTableManager.ItemTable.Get(storeData.productId);
                //         itemBuy.SetItemBuy(itemData);
                //         break;

                    //     default:
                    //         Debug.LogWarning($"알 수 없는 아이템 타입: {storeData.productType}");
                    //         break;
                    // }
            }
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetData(StoreData storeData, GameObject itemBuyPanel, ItemBuy itemBuy, GameObject shopPanel)
    {
        // 데이터 초기화
        this.storeData = storeData;
        this.itemBuyPanel = itemBuyPanel;
        this.itemBuy = itemBuy;
        this.shopPanel = shopPanel;

        var icon = DataTableManager.StringTable.Get(storeData.icon);
        iconImage.sprite = Resources.Load<Sprite>(icon.Value);
    }
}
