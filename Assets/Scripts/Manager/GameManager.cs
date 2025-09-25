using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SlimeManager slimeManager; 
    private GameObject slimeManagerObject; 
    private GameObject environmentManagerObject;
    private EnvironmentManager environmentManager;
    private UiManager uiManager;
    private TimeManager timeManager;
    private GameObject timeManagerObject; // TimeManager 오브젝트 참조
    private GameObject uiManagerObject; // UI 매니저 오브젝트 참조
    public bool IsOneCoin { get; set; }
    public bool IsRespawn { get; set; } = false; // 리스폰 여부
    public bool isFirstStart { get; set; }

    public bool IsCat { get; set; } = false; // 고양이 슬라임 여부

    public Button SaveButton;
    private void Awake()
    {
        // 게임 시작 시 최초 실행 여부 확인
        if (isFirstStart)
        {
            IsOneCoin = true;
            Debug.Log("게임이 처음 시작되었습니다.");
        }
    }
    private void Start()
    {
        SaveButton.onClick.AddListener(saveButton);

        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();

        environmentManagerObject = GameObject.FindWithTag(Tags.EnvironmentManager);
        environmentManager = environmentManagerObject.GetComponent<EnvironmentManager>();

        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        timeManagerObject = GameObject.FindWithTag(Tags.TimeManager);
        timeManager = timeManagerObject.GetComponent<TimeManager>();
        if (timeManager == null)
        {
            Debug.LogWarning("TimeManager를 찾을 수 없습니다!");
        }

        // 환경 변화에 대한 이벤트 구독
        EnvironmentManager.OnEnvironmentChanged += CheckAndDisappearSlime;


        var unLockData = DataTableManager.UnlockConditionTable.Get(DataTableIds.SlimeIds[(int)SlimeType.Plant]);

        // 게임 데이터 로드
        StartCoroutine(DelayedLoadGameData());
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        EnvironmentManager.OnEnvironmentChanged -= CheckAndDisappearSlime;
        
        // 안전한 저장 (오브젝트가 파괴되는 도중에도 안전하게 저장)
        try
        {
            // 애플리케이션이 종료되는 중이 아닐 때만 저장
            if (!Application.isPlaying || this == null) return;
            
            SaveGameData();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"OnDestroy에서 저장 중 오류 발생 (무시됨): {e.Message}");
        }
    }

    // 게임 종료 시 데이터 저장
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
            Debug.Log("게임 포커스 아웃 - 데이터 저장됨");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
        Debug.Log("게임 종료 - 데이터 저장됨");
    }

    public void saveButton()
    {
        SaveGameData();
        Debug.Log("저장 버튼 클릭 - 게임 데이터 저장됨");
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
            Debug.LogWarning("UnlockConditionTable??찾을 ???�습?�다. Normal ?�라?�을 반환?�니??");
            return;
        }

        // 우선순위에 따른 슬라임 타입 결정

        // 1. 오로라 슬라임(Aurora) 
        // 2. 비 슬라임(Rain)  
        // 3. 고양이 슬라임(Cat)
        // 4. 식물 슬라임(Plant)
        // 5. 불 슬라임(Fire)
        // 6. 얼음 슬라임(Ice)
        // 7. 물 슬라임(Water)
        // 8. 빛 슬라임(Light)
        // 9. 어둠 슬라임(Dark)
        SlimeType[] priority = { SlimeType.Aurora, SlimeType.Rain, SlimeType.Cat, SlimeType.Plant, SlimeType.Fire, SlimeType.Ice, SlimeType.Water, SlimeType.Light, SlimeType.Dark };

        foreach (var slimeType in priority)
        {
            int slimeId = DataTableIds.SlimeIds[(int)slimeType];

            if (CheckSlimeUnlockConditions(slimeId, lightStep, humidity, airconTemp, stoveStep, hasFlowerPot))
            {
                Debug.Log($"{slimeType} 슬라임 조건 만족 - 해당 슬라임 활성화");
                slimeManager.slimeType = slimeType;
                return;
            }
        }
        Debug.Log("조건을 만족하는 슬라임이 없음 - 슬라임 활성화 실패");
    }

    // 슬라임의 모든 잠금 조건을 체크하는 메서드
    private bool CheckSlimeUnlockConditions(int slimeId, int lightStep, int humidity, int airconTemp, int stoveStep, bool hasFlowerPot)
    {
        var unlockConditionTable = DataTableManager.UnlockConditionTable;

        // 해당 슬라임의 모든 잠금 조건을 찾아서 체크
        foreach (var unlockId in DataTableIds.UnlockIds)
        {
            var conditionData = unlockConditionTable.Get(unlockId);
            if (conditionData != null && conditionData.SlimeId == slimeId)
            {
                // OptionType이 0이면 잠금 조건을 만족함
                if (conditionData.OptionType == 0)
                {
                    return true;
                }

                // 잠금 조건 체크
                bool conditionMet = CheckUnlockCondition(conditionData, lightStep, humidity, airconTemp, stoveStep, hasFlowerPot);
                if (!conditionMet)
                {
                    return false; // 다른 조건도 만족하지 않으면 잠금 실패
                }
            }
        }

        return true; // 모든 조건이 만족하면 잠금 성공
    }

    // 슬라임의 단일 해금 조건을 체크하는 메서드
    private bool CheckUnlockCondition(UnlockConditionData conditionData, int lightStep, int humidity, int airconTemp, int stoveStep, bool hasFlowerPot)
    {
        switch (conditionData.OptionType)
        {
            case 1: // 조명 조건
                if (!InteriorManager.Instance.GetInteriorLightActive())
                {
                    return false; 
                }
                if (conditionData.SubCondition == 1) // 이상 조건
                {
                    return lightStep >= conditionData.OptionValue;
                }
                else if (conditionData.SubCondition == 2) // 이하 조건
                {
                    return lightStep <= conditionData.OptionValue;
                }
                break;

            case 2: // ?�도 조건
                if (!InteriorManager.Instance.GetHumidifierActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 1) // 이상조건
                    {
                        return humidity >= conditionData.OptionValue;
                    }
                break;

            case 3: // ?�어�?조건
                if (!InteriorManager.Instance.GetAirconActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 2) // 이하 조건
                {
                    return airconTemp <= conditionData.OptionValue;
                }
                break;

            case 4: // 난로 조건
                if (!InteriorManager.Instance.GetStoveActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 1) // 이상 조건
                {
                    return stoveStep >= conditionData.OptionValue;
                }
                break;

            case 10: // ?�분 조건
                if (!InteriorManager.Instance.GetFlowerPotActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 10) // 화분 필요
                    {
                        return hasFlowerPot && conditionData.OptionValue == 1;
                    }
                break;

            case 15:    // ?�실 조건
                if (!InteriorManager.Instance.GetWoolenYarnActive())
                {
                    return false;
                }
                if (IsCat)
                {
                    return false;
                }
                if (conditionData.SubCondition == 10) // 실 필요
                {
                    IsCat = true;
                    return conditionData.OptionValue == 1;
                }
                break;

            case 12: // ?�씨 조건 (�?
                if (!InteriorManager.Instance.GetWindowActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 12) // 조정 날씨
                {
                    // TODO: 날씨 상태 체크 구현
                    return CheckWeatherCondition((int)conditionData.OptionValue);
                }
                break;

            case 13: // ?�간 조건 (?�벽)
                if (!InteriorManager.Instance.GetClockActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 13) // 조정 시간
                {
                    // TODO: 시간 상태 체크 구현
                    return CheckTimeCondition((int)conditionData.OptionValue);
                }
                break;

            case 14: // 복합 조건
                if (!InteriorManager.Instance.GetClockActive() || !InteriorManager.Instance.GetWindowActive())
                {
                    return false;
                }
                if (conditionData.SubCondition == 13) // 조정 시간
                {
                    // TODO: 복합 조건 체크 구현
                    return CheckComplexCondition((int)conditionData.OptionValue);
                }
                break;

            default:
                Debug.LogWarning($"알 수 없는 OptionType: {conditionData.OptionType}");
                return false;
        }

        return false;
    }

    // 날씨 조건 체크 메서드
    private bool CheckWeatherCondition(int weatherType)
    {
        
        switch (timeManager.CurrentWeather)
        {
            case TimeManager.WeatherState.Rain: // �?
                return true;

            case TimeManager.WeatherState.Clear: // 맑음
                return false;
            default:
                Debug.LogWarning($"알 수 없는 weatherType: {weatherType}");
                return false;
        }


    }

    // 시간 조건 체크 메서드
    private bool CheckTimeCondition(int timeType)
    {
        // TimeManager에서 사용하는 시간 상태 체크
        // timeType 13 = 벽 시각
        if (timeType == 13)
        {
            return timeManager.CurrentTimeOfDay == TimeManager.TimeState.Night;
        }
        return false;
    }

    // 복합 조건 체크 메서드
    private bool CheckComplexCondition(int complexType)
    {
        // TODO: 복합 조건 구현
        // complexType 14 = 맑은 날씨 + 시계
        if (complexType == 14)
        {
            return CheckTimeCondition(13) && !CheckWeatherCondition(12); // 시계이면서 맑은 날씨가 아닌 경우
        }
        return false;
    }

    // 소멸 조건을 체크하는 메서드
    public void CheckAndDisappearSlime()
    {
        if (slimeManager != null && slimeManager.HasCurrentSlime())
        {
            // 소멸 조건 체크
            if (slimeManager.ShouldDisappearInCurrentEnvironment(environmentManager))
            {
                if (IsOneCoin)
                {
                    UseSecondChance();
                    IsOneCoin = false;
                    return;
                }
                SlimeType currentType = slimeManager.GetCurrentSlimeType();
                slimeManager.ForceDisappear($"{currentType} 환경 조건 불만족");
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
        // 소멸 조건 체크
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
                foreach (var unlockId in DataTableIds.UnlockIds)
                {
                    var unLockData = DataTableManager.UnlockConditionTable.Get(unlockId);
                    if (unLockData != null && unLockData.SlimeId == DataTableIds.SlimeIds[(int)SlimeType.Ice])
                    {
                        // 습도 조건 체크
                        if (unLockData.DisappearOptionType == 2 && humidity <= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 습도 관측 경고
                            break;
                        }
                        // 온도 조건 체크
                        else if (unLockData.DisappearOptionType == 3 && airconTemp >= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 온도 관측 경고
                            break;
                        }
                    }
                }
                break;
            case SlimeType.Fire:
                id = GetWarningScriptKey((int)SlimeType.Fire);
                break;
            case SlimeType.Plant:
                // 온도 조건 체크
                foreach (var unlockId in DataTableIds.UnlockIds)
                {
                    var unLockData = DataTableManager.UnlockConditionTable.Get(unlockId);
                    if (unLockData != null && unLockData.SlimeId == DataTableIds.SlimeIds[(int)SlimeType.Plant])
                    {
                        // 온도 조건 체크
                        if (unLockData.DisappearOptionType == 2 && stoveStep >= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 온도 관측 경고
                            break;
                        }
                        // 온도 조건 체크
                        else if (unLockData.DisappearOptionType == 10 && lightStep <= unLockData.DisappearOptionValue)
                        {
                            id = unLockData.SlimeWarningScript; // 온도 관측 경고
                            break;
                        }
                    }
                }
                break;
            default:
                break;
        }

        // TODO: 매개변수로 보낼친구 설정해서 보내기
        uiManager.ShowWarningText(id);
        Debug.Log("슬라임 1차 막음");
    }
    // 슬라임 다이 경고 스크립트 가져오기
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

        // GameManager 관련 데이터 저장
        saveData.IsOneCoin = IsOneCoin;
        saveData.IsFirstStart = isFirstStart;

        // SlimeManager 관련 데이터 저장
        if (slimeManager != null)
        {
            saveData.CurrentSlimeId = slimeManager.CurrentSlimeId;
            saveData.CurrentSlimeType = (int)slimeManager.slimeType;
            saveData.SlimeDestroyed = slimeManager.SlimeDestroyed;
            saveData.IsSlimeFree = slimeManager.IsSlimeFree;
            saveData.IsFromSummonStone = slimeManager.IsFromSummonStone; // 소환석 정보 저장
        }

        // SlimeGrowth 관련 데이터 저장
        var currentSlimeObj = GameObject.FindWithTag(Tags.Player);
        var slimeGrowth = currentSlimeObj?.GetComponent<SlimeGrowth>();
        if (slimeGrowth == null && slimeManager.HasCurrentSlime())
        {
            // 현재 슬라임에 SlimeGrowth 컴포넌트 찾기
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

            Debug.Log($"슬라임 성장 정보 저장됨: 레벨 {slimeGrowth.Level}, 경험치 {slimeGrowth.CurrentExp}/{slimeGrowth.MaxExp}");
        }

        // EnvironmentManager 관련 데이터 저장
        if (environmentManager != null)
        {
            saveData.AirconTemp = environmentManager.AirconTemp;
            saveData.StoveStep = environmentManager.StoveStep;
            saveData.LightStep = environmentManager.LightStep;
            saveData.IsFlower = environmentManager.IsFlower;
            saveData.Humidity = environmentManager.Humidity;
            
            // ?�경 ?�브?�트 ?�성 ?�태 ?�??
            environmentManager.SaveEnvironmentObjectStates();
        }

        // CollectionManager ?�이???�???�청
        var collectionManager = GameObject.FindWithTag(Tags.CollectionManager)?.GetComponent<CollectionManager>();
        if (collectionManager != null)
        {
            collectionManager.SaveCollectionData();
        }

        // UiManager ?�이???�???�청
        if (uiManager != null)
        {
            uiManager.SaveUIStates();
        }

        // 게임 진행 데이터 저장
        saveData.GameTime += Time.time;
        saveData.SlimeGenerationCount++;
        saveData.IsCat = IsCat; // 고양이 슬라임 출현 상태 저장
        
        // 화폐 데이터 저장
        if (CurrencyManager.Instance != null)
        {
            saveData.Ether = CurrencyManager.Instance.Ether;
        }
        
        // 시간/날씨 데이터 저장
        var timeManager = GameObject.FindWithTag(Tags.TimeManager)?.GetComponent<TimeManager>();
        if (timeManager != null)
        {
            // TimeManager에서 시간 데이터 가져오기
            saveData.DayCount = timeManager.DayCount;
            saveData.CurrentTime = timeManager.CurrentTime;
            saveData.CurrentTimeOfDay = (int)timeManager.CurrentTimeOfDay;
            saveData.CurrentWeather = (int)timeManager.CurrentWeather;
            saveData.DayDuration = timeManager.DayDuration;
        }
        
        // 인벤토리 데이터 저장
        var invenManager = GameObject.FindWithTag(Tags.InvenManager)?.GetComponent<InvenManager>();
        if (invenManager != null)
        {
            invenManager.SaveInventoryData();
        }
        
        // 인테리어 상태 저장
        try
        {
            if (InteriorManager.Instance != null)
            {
                InteriorManager.Instance.SaveInteriorStates();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"인테리어 상태 저장 중 오류 발생 (무시됨): {e.Message}");
        }

        SaveLoadManager.Save();
        Debug.Log("게임 데이터 저장됨");
    }

    // 게임 데이터 로드
    public void LoadGameData()
    {
        // 저장된 데이터에서 로드 방법
        bool loadSuccess = SaveLoadManager.Load();
        if (!loadSuccess)
        {
            Debug.Log("저장된 데이터 로드 실패. 최초 게임 시작합니다.");

            // 최초 게임 시작 설정
            isFirstStart = true;
            IsOneCoin = true;

            // 기본 슬라임 생성
            if (slimeManager != null)
            {
                slimeManager.CreateSlime(SlimeType.Normal, false, true);
            }
            
            Debug.Log("최초 게임 ?�작 ?�료");
            return;
        }
        
        var saveData = SaveLoadManager.Data;
        Debug.Log($"게임 데이터 로드 작업 - IsFirstStart: {saveData.IsFirstStart}, CurrentSlimeType: {saveData.CurrentSlimeType}, SlimeLevel: {saveData.SlimeLevel}");

        // GameManager 데이터 로드
        IsOneCoin = saveData.IsOneCoin;
        isFirstStart = saveData.IsFirstStart;

        // EnvironmentManager 데이터 로드
        if (environmentManager != null)
        {
            // 환경 오브젝트 상태 로드
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
            // 슬라임 상태 복원
            slimeManager.slimeType = (SlimeType)saveData.CurrentSlimeType;
            slimeManager.SlimeDestroyed = saveData.SlimeDestroyed;
            slimeManager.IsFromSummonStone = saveData.IsFromSummonStone; // 소환석 정보 복원
            Debug.Log($"슬라임 상태 복원: {slimeManager.slimeType}");
            Debug.Log($"슬라임 파괴 여부 복원: {slimeManager.SlimeDestroyed}");
            Debug.Log($"소환석 슬라임 여부 복원: {slimeManager.IsFromSummonStone}");

            // 슬라임이 파괴되지 않았고 슬라임 ID가 존재하는 경우
            if (!saveData.SlimeDestroyed && saveData.CurrentSlimeId != 0)
            {
                Debug.Log($"슬라임 생성 요청: ID={saveData.CurrentSlimeId}, Type={saveData.CurrentSlimeType}");

                // 슬라임 생성 (슬라임이 파괴된 경우 선택 UI 표시함, 생성 메시지 표시함)
                slimeManager.CreateSlime((SlimeType)saveData.CurrentSlimeType, false, false, saveData.IsFromSummonStone);

                // 파괴된 슬라임 ID 설정
                slimeManager.SetCurrentSlimeId(saveData.CurrentSlimeId);

                // 슬라임 성장 데이터 복원 코루틴 실행
                StartCoroutine(RestoreSlimeGrowthData(saveData));
            }
            else
            {
                Debug.Log("슬라임이 파괴된 상태이거나 ID가 존재하지 않습니다.");
            }
        }

        // 화폐 데이터 로드
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.SetEther(saveData.Ether);
        }
        
        // 시간/날씨 데이터 로드
        var timeManager = GameObject.FindWithTag(Tags.TimeManager)?.GetComponent<TimeManager>();
        if (timeManager != null)
        {
            timeManager.LoadTimeData(saveData.CurrentTime, saveData.DayCount, (int)saveData.CurrentTimeOfDay, (int)saveData.CurrentWeather, saveData.DayDuration);
        }
        
        // 인벤토리 데이터 로드
        var invenManager = GameObject.FindWithTag(Tags.InvenManager)?.GetComponent<InvenManager>();
        if (invenManager != null)
        {
            invenManager.LoadInventoryData();
        }
        
        // 인테리어 상태 로드
        if (InteriorManager.Instance != null)
        {
            InteriorManager.Instance.LoadInteriorStates();
        }
        
        // 게임 데이터 로드 완료
        IsCat = saveData.IsCat; // 고양이 슬라임 출현 상태 로드

        // 모든 UI 새로고침
        RefreshAllUI();
        
        // 메일 UI 로드 (UI 새로고침 후에 수행)
        var mailManager = FindFirstObjectByType<MailManager>();
        if (mailManager != null)
        {
            mailManager.LoadMailUI();
        }

        Debug.Log("게임 데이터 로드 완료");
    }

    // 모든 UI를 새로고침하는 메서드
    private void RefreshAllUI()
    {
        // 재화 UI는 CurrencyManager.SetEther()에서 이미 처리됨
        
        // 컬렉션 UI 업데이트 (필요한 경우)
        var collectionManager = FindFirstObjectByType<CollectionManager>();
        if (collectionManager != null)
        {
            collectionManager.UpdateCollectionUI();
        }
        
        Debug.Log("모든 UI 새로고침 완료");
    }

    
    // 슬라임 성장 데이터를 복원하는 코루틴
    private IEnumerator RestoreSlimeGrowthData(SaveDataV1 saveData)
    {
        // 슬라임 성장 데이터 복원까지 대기
        yield return new WaitForSeconds(0.2f);
        
        var currentSlime = slimeManager.GetCurrentSlime();
        if (currentSlime != null)
        {
            var slimeGrowth = currentSlime.GetComponent<SlimeGrowth>();
            if (slimeGrowth != null)
            {
                // 슬라임 성장 데이터 복원
                slimeGrowth.Level = saveData.SlimeLevel;
                slimeGrowth.CurrentExp = saveData.SlimeCurrentExp;
                slimeGrowth.MaxExp = saveData.SlimeMaxExp;
                slimeGrowth.Index = saveData.SlimeLevelIndex;
                slimeGrowth.ScaleLevel = saveData.SlimeScaleLevel;
                slimeGrowth.PreviousScaleLevel = saveData.SlimeScaleLevel;
                // 슬라임 스케일 복원
                currentSlime.transform.localScale = saveData.SlimeScale;

                uiManager.UpdateLevelUI(slimeGrowth.Level);
                slimeGrowth.ExpChanged();
                slimeGrowth.LevelChanged();
                
                Debug.Log($"?�라???�장 ?�이??복원 ?�료: ?�벨 {slimeGrowth.Level}, 경험�?{slimeGrowth.CurrentExp}");
            }
            else
            {
                Debug.LogWarning("SlimeGrowth 컴포?�트�?찾을 ???�습?�다.");
            }
        }
        else
        {
            Debug.LogWarning("?�재 ?�라???�브?�트�?찾을 ???�습?�다.");
        }
    }

    // 지연된 게임 데이터 로드
    private IEnumerator DelayedLoadGameData()
    {
        // 1-2 프레임 대기 (모든 Start 메서드 호출 완료)
        yield return null;
        yield return null;

        // 추가적인 DataTableManager 초기화
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
            Debug.LogWarning("필요한 매니저가 적절히 준비되지 않았습니다. 기본값으로 게임을 시작합니다.");
        }
    }
}