using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlimeManager slimeManager; // 슬라임 참조
    private GameObject slimeManagerObject; // 슬라임 오브젝트 참조
    private GameObject environmentManagerObject;
    private EnvironmentManager environmentManager;
    private UiManager uiManager;
    private GameObject uiManagerObject; // UI 매니저 오브젝트 참조
    public bool IsOneCoin { get; set; }
    public bool IsRespawn { get; set; } = false; // 슬라임 리스폰 여부
    public bool isFirstStart { get; set; }

    private void Awake()
    {
        // 게임 시작 시 최초 실행 여부를 확인
        if (isFirstStart)
        {
            IsOneCoin = true;
            Debug.Log("게임이 처음 시작되었습니다.");
        }
    }
    private void Start()
    {

        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();

        environmentManagerObject = GameObject.FindWithTag(Tags.EnvironmentManager);
        environmentManager = environmentManagerObject.GetComponent<EnvironmentManager>();

        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        // 환경 변화 이벤트 구독
        EnvironmentManager.OnEnvironmentChanged += CheckAndDisappearSlime;

        var unLockData = DataTableManager.UnlockConditionTable.Get(DataTableIds.SlimeIds[(int)SlimeType.Plant]);

        // 게임 데이터 로드
        StartCoroutine(DelayedLoadGameData());
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        EnvironmentManager.OnEnvironmentChanged -= CheckAndDisappearSlime;
        //SaveGameData();
    }

    //게임 종료 시 자동 저장
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGameData();
            Debug.Log("게임 일시정지 - 데이터 저장됨");
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGameData();
            Debug.Log("게임 포커스 잃음 - 데이터 저장됨");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
        Debug.Log("게임 종료 - 데이터 저장됨");
    }


    // 현재 환경 상태에 따른 슬라임 타입 결정
    public void GetSlimeTypeByEnvironment()
    {
        if (environmentManager == null)
        {
            Debug.LogWarning("EnvironmentManager를 찾을 수 없습니다. Normal 슬라임을 반환합니다.");
            slimeManager.slimeType = SlimeType.Normal;
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
                Debug.Log($"{slimeType} 슬라임 조건 만족 - 해당 슬라임 생성");
                slimeManager.slimeType = slimeType;
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
        if (slimeManager != null && slimeManager.HasCurrentSlime())
        {
            // 현재 환경에서 소멸해야 하는지 확인
            if (slimeManager.ShouldDisappearInCurrentEnvironment(environmentManager))
            {
                if (IsOneCoin)
                {
                    UseSecondChance();
                    IsOneCoin = false;
                    return;
                }
                SlimeType currentType = slimeManager.GetCurrentSlimeType();
                slimeManager.ForceDisappear($"{currentType} 슬라임 환경 조건 불만족");
                IsOneCoin = true;
            }
        }
    }
    public void UseSecondChance()
    {
        string id = "";
        int lightStep = environmentManager.LightStep;
        int humidity = environmentManager.Humidity;
        int airconTemp = environmentManager.AirconTemp;
        int stoveStep = environmentManager.StoveStep;
        // 현재 슬라임 타입과 사용된 아이템 아이디를 가져오기
        switch (slimeManager.GetCurrentSlimeType())
        {
            case SlimeType.Light:
                id = GetWarningScriptKey((int)SlimeType.Light);
                break;
            case SlimeType.Dark:
                id = GetWarningScriptKey((int)SlimeType.Dark);
                break;
            case SlimeType.Water:
                id = GetWarningScriptKey((int)SlimeType.Water);
                break;
            case SlimeType.Ice:
                // 아이템 2개, 대사 2개
                foreach (var unlockId in DataTableIds.UnlockIds)
                {
                    var unLockData = DataTableManager.UnlockConditionTable.Get(unlockId);
                    if (unLockData != null && unLockData.SlimeId == DataTableIds.SlimeIds[(int)SlimeType.Ice])
                    {
                        // 습도 조건 체크
                        if (unLockData.DisappearOptionType == 2 && humidity <= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 습도 관련 경고
                            break;
                        }
                        // 온도 조건 체크
                        else if (unLockData.DisappearOptionType == 3 && airconTemp >= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 온도 관련 경고
                            break;
                        }
                    }
                }
                break;
            case SlimeType.Fire:
                id = GetWarningScriptKey((int)SlimeType.Fire);
                break;
            case SlimeType.Plant:
                // 아이템 3개, 대사 2개
                foreach (var unlockId in DataTableIds.UnlockIds)
                {
                    var unLockData = DataTableManager.UnlockConditionTable.Get(unlockId);
                    if (unLockData != null && unLockData.SlimeId == DataTableIds.SlimeIds[(int)SlimeType.Plant])
                    {
                        // 온도 조건 체크
                        if (unLockData.DisappearOptionType == 2 && stoveStep >= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 온도 관련 경고
                            break;
                        }
                        // 습도 조건 체크
                        else if (unLockData.DisappearOptionType == 10 && lightStep <= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 습도 관련 경고
                            break;
                        }
                    }
                }
                break;
            default:
                break;
        }

        // TODO: 매개변수로 보낼친구 선정해서 보내기
        uiManager.ShowWarningText(id);
        Debug.Log("소멸 1회 막음");
    }

    public string GetWarningScriptKey(int slimeId)
    {
        foreach (var unlockId in DataTableIds.UnlockIds)
        {
            var unLockData = DataTableManager.UnlockConditionTable.Get(unlockId);
            if (unLockData != null && unLockData.SlimeId == DataTableIds.SlimeIds[slimeId])
            {

                return unLockData.SlimeWarningScript;
            }
        }
        return "";
    }

    public void SaveGameData()
    {
        var saveData = SaveLoadManager.Data;

        // GameManager 데이터 저장
        saveData.IsOneCoin = IsOneCoin;
        saveData.IsFirstStart = isFirstStart;

        // SlimeManager 데이터 저장
        if (slimeManager != null)
        {
            saveData.CurrentSlimeId = slimeManager.CurrentSlimeId;
            saveData.CurrentSlimeType = (int)slimeManager.slimeType;
            saveData.SlimeDestroyed = slimeManager.SlimeDestroyed;
            saveData.IsSlimeFree = slimeManager.IsSlimeFree;
        }

        // SlimeGrowth 데이터 저장
        var currentSlimeObj = GameObject.FindWithTag(Tags.Player);
        var slimeGrowth = currentSlimeObj?.GetComponent<SlimeGrowth>();
        if (slimeGrowth == null && slimeManager.HasCurrentSlime())
        {
            // 현재 슬라임에서 SlimeGrowth 컴포넌트 찾기
            if (currentSlimeObj != null)
            {
                slimeGrowth = currentSlimeObj.GetComponent<SlimeGrowth>();
            }
        }

        if (slimeGrowth != null)
        {
            saveData.SlimeScale = slimeGrowth.transform.localScale;
            saveData.SlimeLevel = slimeGrowth.Level;
            saveData.SlimeCurrentExp = slimeGrowth.CurrentExp;
            saveData.SlimeMaxExp = slimeGrowth.MaxExp;
            saveData.SlimeLevelIndex = slimeGrowth.Index;
            saveData.SlimeIsStartCoroutine = slimeGrowth.IsStartCoroutine;

            Debug.Log($"슬라임 성장 데이터 저장: 레벨 {slimeGrowth.Level}, 경험치 {slimeGrowth.CurrentExp}/{slimeGrowth.MaxExp}");
        }

        // EnvironmentManager 데이터 저장
        if (environmentManager != null)
        {
            saveData.AirconTemp = environmentManager.AirconTemp;
            saveData.StoveStep = environmentManager.StoveStep;
            saveData.LightStep = environmentManager.LightStep;
            saveData.IsFlower = environmentManager.IsFlower;
            saveData.Humidity = environmentManager.Humidity;
            
            // 환경 오브젝트 활성 상태 저장
            environmentManager.SaveEnvironmentObjectStates();
        }

        // CollectionManager 데이터 저장 요청
        var collectionManager = GameObject.FindWithTag(Tags.CollectionManager)?.GetComponent<CollectionManager>();
        if (collectionManager != null)
        {
            collectionManager.SaveCollectionData();
        }

        // UiManager 데이터 저장 요청
        if (uiManager != null)
        {
            uiManager.SaveUIStates();
        }

        // 게임 진행 통계 저장
        saveData.GameTime += Time.time;
        saveData.SlimeGenerationCount++;

        SaveLoadManager.Save();
        Debug.Log("게임 데이터 저장 완료");
    }

    // 게임 데이터 로드
    public void LoadGameData()
    {
        // 저장된 파일에서 데이터 로드 시도
        bool loadSuccess = SaveLoadManager.Load();
        if (!loadSuccess)
        {
            Debug.Log("저장된 데이터가 없거나 로드 실패. 최초 게임으로 시작합니다.");
            
            // 최초 게임 시작 설정
            isFirstStart = true;
            IsOneCoin = true;
            
            // 기본 슬라임 생성 (최초 실행이므로 선택 UI 없음)
            if (slimeManager != null)
            {
                slimeManager.CreateSlime(SlimeType.Normal, false);
            }
            
            Debug.Log("최초 게임 시작 완료");
            return;
        }
        
        var saveData = SaveLoadManager.Data;
        Debug.Log($"데이터 로드 시작 - IsFirstStart: {saveData.IsFirstStart}, CurrentSlimeType: {saveData.CurrentSlimeType}, SlimeLevel: {saveData.SlimeLevel}");

        // GameManager 데이터 로드
        IsOneCoin = saveData.IsOneCoin;
        isFirstStart = saveData.IsFirstStart;
        
        // EnvironmentManager 데이터 로드
        if (environmentManager != null)
        {
            // 환경 오브젝트 활성 상태 로드
            environmentManager.LoadEnvironmentObjectStates();

            environmentManager.AirconTemp = saveData.AirconTemp;
            environmentManager.StoveStep = saveData.StoveStep;
            environmentManager.LightStep = saveData.LightStep;
            environmentManager.IsFlower = saveData.IsFlower;
            environmentManager.Humidity = saveData.Humidity;
            
            
            
            Debug.Log($"환경 데이터 로드: 에어컨={saveData.AirconTemp}, 스토브={saveData.StoveStep}, 조명={saveData.LightStep}, 화분={saveData.IsFlower}, 습도={saveData.Humidity}");
        }

        // CollectionManager 데이터 로드 요청
        var collectionManager = GameObject.FindWithTag(Tags.CollectionManager)?.GetComponent<CollectionManager>();
        if (collectionManager != null)
        {
            collectionManager.LoadCollectionData();
        }

        // UiManager 데이터 로드 요청
        if (uiManager != null)
        {
            uiManager.LoadUIStates();
        }

        // SlimeManager 데이터 로드
        if (slimeManager != null)
        {
            // 슬라임 타입 복원
            slimeManager.slimeType = (SlimeType)saveData.CurrentSlimeType;
            Debug.Log($"슬라임 타입 복원: {slimeManager.slimeType}");

            // 슬라임이 파괴된 상태가 아니고 유효한 슬라임 ID가 있다면 생성
            if (!saveData.SlimeDestroyed && saveData.CurrentSlimeId != 0)
            {
                Debug.Log($"슬라임 생성 시도: ID={saveData.CurrentSlimeId}, Type={saveData.CurrentSlimeType}");

                // 슬라임 생성 (저장된 타입으로, 선택 UI 표시 안함)
                slimeManager.CreateSlime((SlimeType)saveData.CurrentSlimeType, false);

                // 저장된 슬라임 ID로 재설정
                slimeManager.SetCurrentSlimeId(saveData.CurrentSlimeId);

                // 슬라임 성장 데이터 복원을 코루틴으로 지연 실행
                StartCoroutine(RestoreSlimeGrowthData(saveData));
            }
            else
            {
                Debug.Log("슬라임이 파괴된 상태이거나 유효하지 않은 ID입니다.");
            }
        }

        

        Debug.Log("게임 데이터 로드 완료");
    }
    
    // 슬라임 성장 데이터 복원을 위한 코루틴
    private IEnumerator RestoreSlimeGrowthData(SaveDataV1 saveData)
    {
        // 슬라임이 완전히 생성될 때까지 대기
        yield return new WaitForSeconds(0.2f);
        
        var currentSlime = slimeManager.GetCurrentSlime();
        if (currentSlime != null)
        {
            var slimeGrowth = currentSlime.GetComponent<SlimeGrowth>();
            if (slimeGrowth != null)
            {
                // 성장 데이터 복원
                slimeGrowth.Level = saveData.SlimeLevel;
                slimeGrowth.CurrentExp = saveData.SlimeCurrentExp;
                slimeGrowth.MaxExp = saveData.SlimeMaxExp;
                slimeGrowth.Index = saveData.SlimeLevelIndex;
                slimeGrowth.IsStartCoroutine = saveData.SlimeIsStartCoroutine;
                slimeGrowth.ScaleLevel = saveData.SlimeScaleLevel;
                slimeGrowth.PreviousScaleLevel = saveData.SlimeScaleLevel;
                // 슬라임 스케일 복원
                currentSlime.transform.localScale = saveData.SlimeScale;

                uiManager.UpdateLevelUI(slimeGrowth.Level);
                slimeGrowth.ExpChanged();
                slimeGrowth.LevelChanged();
                
                Debug.Log($"슬라임 성장 데이터 복원 완료: 레벨 {slimeGrowth.Level}, 경험치 {slimeGrowth.CurrentExp}");
            }
            else
            {
                Debug.LogWarning("SlimeGrowth 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("현재 슬라임 오브젝트를 찾을 수 없습니다.");
        }
    }
    
    // 지연된 게임 데이터 로드
    private IEnumerator DelayedLoadGameData()
    {
        // 1-2프레임 대기 (모든 Start 메서드 완료 대기)
        yield return null;
        yield return null;
        
        // 추가로 DataTableManager 초기화 대기
        while (DataTableManager.SlimeTable == null || DataTableManager.UnlockConditionTable == null)
        {
            yield return null;
        }
        
        // 모든 매니저가 준비되었는지 확인
        if (slimeManager != null && environmentManager != null && uiManager != null)
        {
            LoadGameData();
        }
        else
        {
            Debug.LogWarning("일부 매니저가 아직 준비되지 않았습니다. 기본값으로 게임을 시작합니다.");
        }
    }
}