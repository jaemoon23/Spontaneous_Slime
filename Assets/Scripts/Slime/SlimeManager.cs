using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public enum SlimeType
{
    Normal,
    Light,
    Dark,
    Water,
    Ice,
    Fire,
    Plant,
}
// TODO: Debug.Log 제거 및 주석 정리
public class SlimeManager : MonoBehaviour
{
    private SlimeData slimeData;
    public int CurrentSlimeId { get; private set; } // 현재 슬라임 ID
    public string StringScript { get; private set; } // 현재 슬라임 스크립트
    [SerializeField] private int type = 0; // 슬라임 타입 설정용
    public bool SlimeDestroyed { get; private set; } = false;
    private GameObject slimePrefab;
    private GameObject currentSlime; // 현재 생성된 슬라임 오브젝트
    private SlimeGrowth slimeGrowth;
    private float time = 0f; // 슬라임 소멸 후 시간 측정용

    public SlimeType slimeType; // 현재 슬라임 타입
    private SlimeType previousSlimeType; // 이전 슬라임 타입 저장용
    [SerializeField] private GameObject choiceUiObject;
    GameManager gameManager;
    GameObject gameManagerObject;
    private GameObject uiManagerObject;
    private UiManager uiManager; // UI 매니저 참조
    private Reward reward;
    private CollectionManager collectionManager;
    private GameObject collectionManagerObject;
    public bool IsSlimeFree { get; private set; } = false;
    public string[] StringScripts { get; private set; } = new string[0];
    [SerializeField] private GameObject SlimeDieText;
    [SerializeField] private float fadeOutDuration = 2f; // 페이드 아웃 지속 시간

    private Coroutine fadeOutCoroutine;

    public Material[] slimeExpressions; // 슬라임 타입별 머티리얼 배열
    int expressionIndex = 0; // 현재 머티리얼 인덱스
    private GameObject slimeExpressionObject; // 슬라임 오브젝트 참조

    
    private void Awake()
    {
        SlimeDestroyed = false;
    }

    private void Start()
    {
        SlimeGrowth.OnSlimeMaxLevel += CheckSlimeGrowth;
        // 게임 매니저 참조 가져오기
        gameManagerObject = GameObject.FindWithTag(Tags.GameManager);
        gameManager = gameManagerObject.GetComponent<GameManager>();

        // reward 참조 가져오기
        reward = gameObject.GetComponent<Reward>();

        // UI 매니저 참조 가져오기
        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        // 도감 매니저 참조 가져오기
        collectionManagerObject = GameObject.FindWithTag(Tags.CollectionManager);
        collectionManager = collectionManagerObject.GetComponent<CollectionManager>();


        // 슬라임 프리팹 로드
        slimePrefab = Resources.Load<GameObject>(Paths.Slime);
        SlimeDieText.SetActive(false);
    }
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        SlimeGrowth.OnSlimeMaxLevel -= CheckSlimeGrowth;
    }

    private void Update()
    {
        // 슬라임 소멸 후 재생성 처리;
        if (IsSlimeFree)
        {
            gameManager.GetSlimeTypeByEnvironment();
            if (slimeType == SlimeType.Normal || previousSlimeType == slimeType)
            {
                return;
            }
            else
            {
                gameManager.IsRespawn = true;
            }
            
        }
        if (SlimeDestroyed)
        {
            gameManager.GetSlimeTypeByEnvironment();
            if (slimeType == SlimeType.Normal || previousSlimeType == slimeType)
            {
                return; // 기본 슬라임이면 아무 작업도 하지 않음
            }
            else
            {
                gameManager.IsRespawn = true;
            }
        }

        if (gameManager.IsRespawn)
        {
            RespawnSlime();
            gameManager.IsRespawn = false;
        }
    }

    public void CheckSlimeGrowth()
    {
        slimeGrowth = currentSlime.GetComponent<SlimeGrowth>();
        if (slimeGrowth != null && slimeGrowth.Level >= slimeGrowth.MaxLevel)
        {
            // 보상 지급
            if (reward != null)
            {
                reward.GiveReward(slimeData.GiftItemId);
            }
            else
            {
                Debug.LogWarning("Reward 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }

    public void RespawnSlime()
    {
        time += Time.deltaTime;
        if (time > 3f) // currentSlime == null 조건 제거
        {
            // 환경 조건에 맞춰서 슬라임 생성
            CreateSlime(slimeType);
            uiManager.DisableExpUI(true);
            time = 0f;
            SlimeDestroyed = false;
            IsSlimeFree = false;
        }
    }

    public void DestroySlime()
    {
        if (currentSlime != null)
        {
            previousSlimeType = slimeType;
            if (collectionManager != null)
            {
                collectionManager.AddCollection(slimeData);
            }
            else
            {
                Debug.LogWarning("CollectionManager를 찾을 수 없습니다!");
            }
            Destroy(currentSlime);
            currentSlime = null;
            SlimeDestroyed = true;
        }
    }

    public void SlimeFree()
    {
        if (currentSlime != null)
        {
            previousSlimeType = slimeType; // 이전 슬라임 타입 저장
            Destroy(currentSlime);
            currentSlime = null;
            IsSlimeFree = true;
        }
    }

    public void CreateSlime(SlimeType slimeType, bool showChoiceUI = true)
    { //Vector3(c)2.8599999,1.17999995,2.75
        // 슬라임 생성
        currentSlime = Instantiate(slimePrefab, new Vector3(5.67f, 2.8f, 5.47f), Quaternion.Euler(0, 0, 0));
        slimeExpressionObject = GameObject.FindWithTag(Tags.PlayerExpression);
        if (gameManager.isFirstStart)
        {
            type = (int)SlimeType.Normal; // 게임이 처음 시작되었을 때는 기본 슬라임 생성
            gameManager.isFirstStart = false; // 첫 시작 플래그 해제
        }
        else
        {
            // showChoiceUI가 true일 때만 선택 UI 표시 (로드 시에는 false)
            if (showChoiceUI)
            {
                choiceUiObject.SetActive(true);
            }
            type = (int)slimeType;
        }

        // 슬라임 데이터 가져오기
        slimeData = DataTableManager.SlimeTable.Get(DataTableIds.SlimeIds[type]);

        if (slimeData != null)
        {
            CurrentSlimeId = slimeData.SlimeId;     // 현재 슬라임 ID 저장

            // 문자열 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(slimeData.SlimeNameId);
            string[] scriptIds = slimeData.GetScriptIds();

            
            StringScripts = new string[scriptIds.Length]; // 슬라임 스크립트 배열 초기화
            for (int i = 0; i < scriptIds.Length; i++)
            {
                var scriptData = DataTableManager.StringTable.Get(scriptIds[i]);
                if (scriptData != null)
                {
                    StringScripts[i] = scriptData.Value; // 현재 슬라임 스크립트 저장
                }
                else
                {
                    Debug.LogWarning($"스크립트 ID '{scriptIds[i]}'에 대한 StringData를 찾을 수 없습니다!");
                }
            }
            if (stringData != null)
            {
                
                // UI 매니저를 통해 슬라임 등장 텍스트 표시
                if (uiManager != null)
                {
                    uiManager.ShowSlimeSpawnText(stringData.Value);
                }
            }
        }
        
    }

    // 현재 슬라임이 있는지 확인
    public bool HasCurrentSlime()
    {
        return currentSlime != null;
    }

    //TODO: 현재 슬라임 오브젝트 반환
    public GameObject GetCurrentSlime()
    {
        return currentSlime;
    }
    // 현재 슬라임 타입 반환
    public SlimeType GetCurrentSlimeType()
    {
        return (SlimeType)type;
    }

    // 슬라임을 강제로 소멸시키는 메서드 (환경 조건 불만족 시)
    public void ForceDisappear(string reason = "환경 조건 불만족")
    {
        // 이미 실행 중인 페이드 아웃 코루틴이 있으면 중지
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(ShowSlimeDieTextWithFadeOut());

        if (currentSlime != null)
        {
            previousSlimeType = slimeType; // 이전 슬라임 타입 저장
            Destroy(currentSlime);
            currentSlime = null;
            SlimeDestroyed = true;
        }
    }
    
    // 슬라임 죽음 텍스트를 표시하고 페이드 아웃하는 코루틴
    private IEnumerator ShowSlimeDieTextWithFadeOut()
    {
        SlimeDieText.SetActive(true);
        
        // CanvasGroup 컴포넌트 가져오기 (없으면 추가)
        CanvasGroup canvasGroup = SlimeDieText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = SlimeDieText.AddComponent<CanvasGroup>();
        }
        
        // 초기 알파값을 1로 설정
        canvasGroup.alpha = 1f;
        
        // 페이드 아웃 실행
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }
        
        // 완전히 투명해지면 오브젝트 비활성화
        canvasGroup.alpha = 0f;
        SlimeDieText.SetActive(false);
    }

    // 현재 환경에서 슬라임이 소멸해야 하는지 확인
    public bool ShouldDisappearInCurrentEnvironment(EnvironmentManager environmentManager)
    {
        if (environmentManager == null || !HasCurrentSlime())
        {
            return false;
        }

        SlimeType currentType = GetCurrentSlimeType();
        
        // 기본 슬라임은 소멸하지 않음
        if (currentType == SlimeType.Normal)
        {
            return false;
        }
        
        // 현재 환경 상태
        int lightStep = environmentManager.LightStep;
        int humidity = environmentManager.Humidity;
        int airconTemp = environmentManager.AirconTemp;
        int stoveStep = environmentManager.StoveStep;
        bool hasFlowerPot = environmentManager.IsFlower;

        Debug.Log($"소멸 체크 - 슬라임: {currentType}, 조명: {lightStep}, 습도: {humidity}%, 에어컨 온도: {airconTemp}°C, 난로 온도: {stoveStep}°C, 화분: {hasFlowerPot}");

        // CSV 데이터에서 현재 슬라임의 소멸 조건들을 가져와서 체크
        var unlockConditionTable = DataTableManager.UnlockConditionTable;
        if (unlockConditionTable == null)
        {
            Debug.LogWarning("UnlockConditionTable을 찾을 수 없습니다.");
            return false;
        }

        // 현재 슬라임 ID와 일치하는 모든 조건을 확인
        foreach (var unlockId in DataTableIds.UnlockIds)
        {
            var conditionData = unlockConditionTable.Get(unlockId);
            if (conditionData != null && conditionData.SlimeId == CurrentSlimeId)
            {
                Debug.Log($"언락 {conditionData.SlimeId} 슬라임 {CurrentSlimeId}");
                // DisappearOptionType에 따라 소멸 조건 체크
                bool shouldDisappear = Check(conditionData, lightStep, humidity, airconTemp, stoveStep, hasFlowerPot);
                if (shouldDisappear)
                {
                    Debug.Log($"슬라임 {CurrentSlimeId} 소멸 조건 만족: 타입 {conditionData.DisappearOptionType}, 값 {conditionData.DisappearOptionValue}");
                    return true;
                }
            }
        }
        return false;
    }

    // 소멸 조건 체크 메서드 
    private bool Check(UnlockConditionData conditionData, int lightStep, int humidity, int airconTemp, int stoveStep, bool hasFlowerPot)
    {
        // DisappearOptionType에 따라 조건 체크
        switch (conditionData.DisappearOptionType)
        {
            // DisappearSubCondition = 1 이상 2 이하
            case 1: // 조명 조건
                if (conditionData.DisappearSubCondition == 1) // 이상 조건
                {
                    return lightStep >= conditionData.DisappearOptionValue;
                }
                else if (conditionData.DisappearSubCondition == 2) // 이하 조건
                {
                    return lightStep <= conditionData.DisappearOptionValue;
                }
                break;

            case 2: // 습도 조건
                if (conditionData.DisappearSubCondition == 2) // 이하 조건
                {
                    return humidity <= conditionData.DisappearOptionValue;
                }
                break;

            case 3: // 에어컨 조건
                if (conditionData.DisappearSubCondition == 1) // 이상 조건
                {
                    return airconTemp >= conditionData.DisappearOptionValue;
                }
                break;

            case 4: // 화력 조건
                if (conditionData.DisappearSubCondition == 2) // 이하 조건
                {
                    return stoveStep <= conditionData.DisappearOptionValue;
                }
                break;

            case 11: // 화분 조건
                return !hasFlowerPot && conditionData.DisappearOptionValue == 0;

            default:
                return false;
        }

        return false;
    }

    public void ChangeSlimeExpression()
    {
        expressionIndex = (expressionIndex + 1) % slimeExpressions.Length;
        slimeExpressionObject.GetComponent<MeshRenderer>().material = slimeExpressions[expressionIndex];
    }
    
    // 저장된 데이터 로드를 위한 setter 메서드
    public void SetCurrentSlimeId(int slimeId)
    {
        CurrentSlimeId = slimeId;
    }
}
