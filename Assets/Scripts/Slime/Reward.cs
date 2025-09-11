using System.Collections.Generic;
using UnityEngine;

//TDOD: Debug.Log 제거
public class Reward : MonoBehaviour
{
    [SerializeField] private string rewardItemId; // 보상 아이템 ID
    public SlimeManager slime; // 슬라임 참조
    public SlimeGrowth slimeGrowth; // 슬라임 성장 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    private void Start()
    {
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();
    }

    public void GiveReward()
    {
        var itemData = DataTableManager.ItemTable.Get(slime.GiftId);
        
        if (itemData != null)
        {
            string itemName = itemData.ItemName;
            Debug.Log($"선물 아이템 발견! ID: {itemData.ItemId}, 이름: {itemName}");
            
            // 문자열 테이블에서 직접 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(itemName);
            if (stringData != null)
            {
                Debug.Log($"레벨 10 달성! 보상 아이템 지급: {stringData.Value}");
                //TODO: 여기서 아이템 지급
            }
        }
        else
        {
            Debug.LogWarning($"선물 아이템을 찾을 수 없습니다. ID: {slime.GiftId}");
        }
    }
}
