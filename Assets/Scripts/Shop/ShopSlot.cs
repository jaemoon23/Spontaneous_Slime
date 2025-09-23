using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private Button button;
    public GameObject itemBuyPanel;
    public ItemData itemData;
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
            itemBuyPanel.SetActive(true);
            var itemBuy = itemBuyPanel.GetComponent<ItemBuy>();
            if (itemBuy != null)
            {
                //itemBuy.setItemBuy(itemData);
            }
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}
