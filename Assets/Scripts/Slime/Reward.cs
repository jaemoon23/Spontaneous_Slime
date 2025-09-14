using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//TDOD: Debug.Log 제거
public class Reward : MonoBehaviour
{
    public SlimeManager slime; // 슬라임 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    [SerializeField] private ItemMapping[] items;
    public Dictionary<string, GameObject> itemObjects = new Dictionary<string, GameObject>();

    private void Start()
    {
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();

        foreach (var item in items)
        {
            AddItemObject(item.itemId, item.itemObject);
            item.itemObject.SetActive(false); // 처음에는 비활성화
        }
    }

    public void GiveReward(string itemId)
    {
        var itemData = DataTableManager.ItemTable.Get(itemId);

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
                if (itemObjects.TryGetValue(itemId, out var itemObj))
                {
                    itemObj.SetActive(true); // 아이템 오브젝트 활성화
                    Debug.Log($"아이템 오브젝트 활성화: {itemId}");
                }
                else
                {
                    Debug.LogWarning($"아이템 오브젝트를 찾을 수 없습니다. 태그: {itemId}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"선물 아이템을 찾을 수 없습니다. ID: {itemData.ItemName}");
        }
    }

    public void AddItemObject(string itemId, GameObject obj)
    {
        if (!itemObjects.ContainsKey(itemId))
        {
            itemObjects.Add(itemId, obj);
        }
    }
}


