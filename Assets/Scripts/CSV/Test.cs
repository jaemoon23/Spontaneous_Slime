using UnityEngine;

public class Test : MonoBehaviour
{
    public string slimeTestId = "11011";
    public string itemTestId = "20001";

    void Start()
    {
        // 슬라임 테이블 테스트
        var table = DataTableManager.SlimeTable;
        if (table == null)
        {
            Debug.LogError("[Tester] SlimeTable null");
            return;
        }

        var data = table.Get(slimeTestId);
        if (data == null)
        {
            Debug.LogError($"[Tester] ID 없음: {slimeTestId}");
        }
        else
        {
            Debug.Log($"[Tester] {data}");
        }

        // 아이템 테이블 테스트
        var itemTable = DataTableManager.ItemTable;
        if (itemTable == null)
        {
            Debug.LogError("[Tester] ItemTable null");
            return;
        }

        var itemData = itemTable.Get(itemTestId);
        if (itemData == null)
        {
            Debug.LogError($"[Tester] ID 없음: {itemTestId}");
        }
        else
        {
            Debug.Log($"[Tester] {itemData}");
        }
    }
}
