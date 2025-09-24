using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    
    private readonly string slotNamePrefix = "ShopSlot_";

    // ShopSlot 프리팹, 컨텐츠 트랜스폼
    private GameObject shopSlotPrefab;
    [SerializeField] private Transform shopContentTransform;
    [SerializeField] private GameObject itemBuyPanel;
    ItemBuy itemBuy;
    
    private List<ShopSlot> shopSlots = new List<ShopSlot>();

    private void Start()
    {
        itemBuy = itemBuyPanel.GetComponent<ItemBuy>();
        shopSlotPrefab = Resources.Load<GameObject>(Paths.ShopSlot);


        for (int i = 0; i < DataTableIds.StoreIds.Length; i++)
        {
            GameObject slot = Instantiate(shopSlotPrefab, shopContentTransform);
            slot.name = slotNamePrefix + i;
            var shopSlot = slot.GetComponent<ShopSlot>();
        
            var storeData = DataTableManager.StoreTable.Get(DataTableIds.StoreIds[i]);
            shopSlot.SetData(storeData, itemBuyPanel, itemBuy);
            
            shopSlots.Add(shopSlot);      
            slot.SetActive(true);
        }
    }
}
