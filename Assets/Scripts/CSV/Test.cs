using UnityEngine;

public class Test : MonoBehaviour
{
    public string slimeTestId = "11011";
    public string itemTestId = "20001";
    public string levelTestId = "10010001";
    public string unlockConditionTestId = "101001";
    public string stringTestKey = "SLIME_NAME_11011";

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

        // levelUP 테이블 테스트
        var levelUpTable = DataTableManager.LevelUpTable;
        if (levelUpTable == null)
        {
            Debug.LogError("[Tester] levelUpTable null");
            return;
        }

        var levelUpData = levelUpTable.Get(levelTestId);
        if (levelUpData == null)
        {
            Debug.LogError($"[Tester] ID 없음: {levelTestId}");
        }
        else
        {
            Debug.Log($"[Tester] {levelUpData}");
        }

        // UnlockCondition 테이블 테스트
        var unlockConditionTable = DataTableManager.UnlockConditionTable;
        if (unlockConditionTable == null)
        {
            Debug.LogError("[Tester] unlockConditionTable null");
            return;
        }
        var unlockConditionData = unlockConditionTable.Get(unlockConditionTestId);
        if (unlockConditionData == null)
        {
            Debug.LogError($"[Tester] ID 없음: {unlockConditionTestId}");
        }
        else
        {
            Debug.Log($"[Tester] {unlockConditionData}");
        }

        // String 테이블 테스트
        var stringTable = DataTableManager.StringTable;
        if (stringTable == null)
        {
            Debug.LogError("[Tester] StringTable null");
            return;
        }

        var stringData = stringTable.Get(stringTestKey);
        if (stringData == null)
        {
            Debug.LogError($"[Tester] ID 없음: {stringTestKey}");
        }
        else
        {
            Debug.Log($"[Tester] {stringData}");
        }
    }
}
