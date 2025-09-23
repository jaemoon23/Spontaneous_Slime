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
    
    private List<ShopSlot> shopSlots = new List<ShopSlot>();

    private void Start()
    {
        shopSlotPrefab = Resources.Load<GameObject>(Paths.ShopSlot);


        for (int i = 0; i < 30; i++)
        {
            GameObject slot = Instantiate(shopSlotPrefab, shopContentTransform);
            slot.name = slotNamePrefix + i;
            var shopSlot = slot.GetComponent<ShopSlot>();
        
            // var itemData = DataTableManager.ItemTable.Get($"ITEM_{i}");
            // shopSlot.itemData = itemData;
            shopSlot.itemBuyPanel = itemBuyPanel;
            
            shopSlots.Add(shopSlot);      
            slot.SetActive(false);
        }
    }
}
