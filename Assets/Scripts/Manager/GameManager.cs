using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlimeManager slime; // 슬라임 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    private EnvironmentManager environmentManager;
    private GameObject environmentManagerObject;

    public bool IsRespawn { get; set; } = false; // 슬라임 리스폰 여부
    public bool isFirstStart { get; set; } = true;



    private void Awake()
    {
        // 게임 시작 시 최초 실행 여부를 확인
        if (isFirstStart)
        {
            isFirstStart = true;
            Debug.Log("게임이 처음 시작되었습니다.");
        }
    }
    private void Start()
    {

        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();

        environmentManagerObject = GameObject.FindWithTag(Tags.EnvironmentManager);
        environmentManager = environmentManagerObject.GetComponent<EnvironmentManager>();

        environmentManager = FindFirstObjectByType<EnvironmentManager>();

        // 환경 변화 이벤트 구독
        EnvironmentManager.OnEnvironmentChanged += CheckAndDisappearSlime;

        var unLockData = DataTableManager.UnlockConditionTable.Get(DataTableIds.SlimeIds[(int)SlimeType.Plant]);
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        EnvironmentManager.OnEnvironmentChanged -= CheckAndDisappearSlime;
    }


    // 현재 환경 상태에 따른 슬라임 타입 결정
    public void GetSlimeTypeByEnvironment()
    {
        if (environmentManager == null)
        {
            Debug.LogWarning("EnvironmentManager를 찾을 수 없습니다. Normal 슬라임을 반환합니다.");
            slime.slimeType = SlimeType.Normal;
            IsRespawn = true;
            return;
        }

        // 현재 환경 상태
        int lightStep = environmentManager.LightStep;   // 조명 단계 0~5
        int humidity = environmentManager.Humidity; // 습도
        int airconTemp = environmentManager.AirconTemp;
        int stoveStep = environmentManager.StoveStep; // 난로 단계 0~5
        bool hasFlowerPot = environmentManager.IsFlower; // 화분 존재 여부

        var unlockConditionTable = DataTableManager.UnlockConditionTable;
        if (unlockConditionTable == null)
        {
            Debug.LogWarning("UnlockConditionTable을 찾을 수 없습니다. Normal 슬라임을 반환합니다.");
            return;
        }

        // 우선순위에 따른 슬라임 타입 결정

        // 1. 식물 슬라임 (Plant)
        // 2. 불 슬라임 (Fire)
        // 3. 얼음 슬라임 (Ice)
        // 4. 물 슬라임 (Water)
        // 5. 빛 슬라임 (Light)
        // 6. 어둠 슬라임 (Dark)
        SlimeType[] priority = { SlimeType.Plant, SlimeType.Fire, SlimeType.Ice, SlimeType.Water, SlimeType.Light, SlimeType.Dark };

        foreach (var slimeType in priority)
        {
            int slimeId = DataTableIds.SlimeIds[(int)slimeType];

            if (CheckSlimeUnlockConditions(slimeId, lightStep, humidity, airconTemp, stoveStep, hasFlowerPot))
            {
                Debug.Log($"{slimeType} 슬라임 해금 조건 만족");
                slime.slimeType = slimeType;
                return;
            }
        }
        Debug.Log("조건을 만족하는 슬라임이 없음 - 슬라임 생성하지 않음");
    }

    // 슬라임의 모든 해금 조건을 체크하는 메서드
    private bool CheckSlimeUnlockConditions(int slimeId, int lightStep, int humidity, int airconTemp, int stoveStep, bool hasFlowerPot)
    {
        var unlockConditionTable = DataTableManager.UnlockConditionTable;
        
        // 해당 슬라임의 모든 해금 조건을 찾아서 체크
        foreach (var unlockId in DataTableIds.UnlockIds)
        {
            var conditionData = unlockConditionTable.Get(unlockId);
            if (conditionData != null && conditionData.SlimeId == slimeId)
            {
                // OptionType이 0이면 해금 조건이 없음
                if (conditionData.OptionType == 0)
                {
                    return true;
                }

                // 해금 조건 체크
                bool conditionMet = CheckUnlockCondition(conditionData, lightStep, humidity, airconTemp, stoveStep, hasFlowerPot);
                if (!conditionMet)
                {
                    return false; // 하나라도 만족하지 않으면 해금 실패
                }
            }
        }

        return true; // 모든 조건을 만족했거나 조건이 없음
    }
        
    // 단일 해금 조건을 체크하는 메서드
    private bool CheckUnlockCondition(UnlockConditionData conditionData, int lightStep, int humidity, int airconTemp, int stoveStep, bool hasFlowerPot)
    {
        switch (conditionData.OptionType)
        {
            case 1: // 조명 조건
                if (conditionData.SubCondition == 1) // 이상 조건
                {
                    return lightStep >= conditionData.OptionValue;
                }
                else if (conditionData.SubCondition == 2) // 이하 조건
                {
                    return lightStep <= conditionData.OptionValue;
                }
                break;

            case 2: // 습도 조건
                if (conditionData.SubCondition == 1) // 이상 조건
                {
                    return humidity >= conditionData.OptionValue;
                }
                break;

            case 3: // 에어컨 조건
                if (conditionData.SubCondition == 2) // 이하 조건
                {
                    return airconTemp <= conditionData.OptionValue;
                }
                break;

            case 4: // 화력 조건
                if (conditionData.SubCondition == 1) // 이상 조건
                {
                    return stoveStep >= conditionData.OptionValue;
                }
                break;

            case 10: // 화분 조건
                if (conditionData.SubCondition == 10) // 화분 필요
                {
                    return hasFlowerPot && conditionData.OptionValue == 1;
                }
                break;

            default:
                Debug.LogWarning($"알 수 없는 OptionType: {conditionData.OptionType}");
                return false;
        }

        return false;
    }
    

    // 현재 환경에서 슬라임 소멸 조건을 체크하고 필요시 소멸시키는 메서드
    public void CheckAndDisappearSlime()
    {
        if (slime != null && slime.HasCurrentSlime())
        {
            // 현재 환경에서 소멸해야 하는지 확인
            if (slime.ShouldDisappearInCurrentEnvironment(environmentManager))
            {
                SlimeType currentType = slime.GetCurrentSlimeType();
                slime.ForceDisappear($"{currentType} 슬라임 환경 조건 불만족");
            }
        }
    }
}