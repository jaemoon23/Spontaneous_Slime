using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//TDOD: Debug.Log 제거
public class Reward : MonoBehaviour
{
    public SlimeManager slime; // 슬라임 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    [SerializeField] private InteriorMapping[] interiors;
    public Dictionary<int, GameObject> itemObjects = new Dictionary<int, GameObject>();

    private void Start()
    {
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();

        foreach (var interior in interiors)
        {
            AddItemObject(interior.interiorId, interior.interiorObject);
            interior.interiorObject.SetActive(false); // 처음에는 비활성화
        }
    }

    public void GiveReward(int interiorId)
    {
        var interiorData = DataTableManager.InteriorTable.Get(interiorId);

        if (interiorData != null)
        {
            string InteriorName = interiorData.InteriorName;
            Debug.Log($"선물 아이템 발견! ID: {interiorData.InteriorId}, 이름: {InteriorName}");

            // 문자열 테이블에서 직접 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(InteriorName);
            if (stringData != null)
            {
                Debug.Log($"레벨 10 달성! 보상 아이템 지급: {stringData.Value}");
                //TODO: 여기서 아이템 지급
                if (itemObjects.TryGetValue(interiorId, out var itemObj))
                {
                    itemObj.SetActive(true); // 아이템 오브젝트 활성화
                    Debug.Log($"아이템 오브젝트 활성화: {interiorId}");
                }
                else
                {
                    Debug.LogWarning($"아이템 오브젝트를 찾을 수 없습니다. 태그: {interiorId}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"선물 아이템을 찾을 수 없습니다. ID: {interiorId}");
        }
    }

    public void AddItemObject(int interiorId, GameObject obj)
    {
        if (!itemObjects.ContainsKey(interiorId))
        {
            itemObjects.Add(interiorId, obj);
        }
    }
}


