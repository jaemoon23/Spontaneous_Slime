using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private List<ItemData> itemDataList = new List<ItemData>(); // 아이템 데이터 리스트
    [SerializeField] private string rewardItemId; // 보상 아이템 ID
    public Slime slime; // 슬라임 참조
    public SlimeGrowth slimeGrowth; // 슬라임 성장 참조
    private void Start()
    {
        slime = GetComponent<Slime>();
        slimeGrowth = GetComponent<SlimeGrowth>();
        foreach (var id in DataTableIds.ItemIds)
        {
            var itemData = DataTableManager.ItemTable.Get(id);
            itemDataList.Add(itemData);
        }
        
        
    }

    public void GiveReward()
    {
        foreach (var item in itemDataList)
        {
            if (item.ItemId == slime.GiftId)
            {
                rewardItemId = item.ItemName;
                Debug.Log($"선물 아이템 발견! ID: {item.ItemId}, 이름: {item.ItemName}");
                break;
            }
            else
            {
                return;
            }
        }
        Debug.Log($"Reward에서 할당된 rewardItemId: {rewardItemId}");
        foreach (var item in slime.StringDataList)
        {
            //Debug.Log($"슬라임 StringDataList 아이템: {item.key}, {item.Value}");
            if (item.key == rewardItemId)
            {
                Debug.Log($"레벨 10 달성! 보상 아이템 지급: {item.Value}");
                break;
            }
        }
    }
}
