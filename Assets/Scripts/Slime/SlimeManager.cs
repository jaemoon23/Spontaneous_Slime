using NUnit.Framework;
using TMPro;
using UnityEngine;

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
    public string CurrentSlimeId { get; private set; } // 현재 슬라임 ID
    public string StringScripts { get; private set; } // 현재 슬라임 스크립트
    [SerializeField] private int type = 0; // 슬라임 타입 설정용
    public bool SlimeDestroyed { get; private set; } = false;
    private GameObject slimePrefab;
    private GameObject currentSlime; // 현재 생성된 슬라임 오브젝트
    private SlimeGrowth slimeGrowth;
    private float time = 0f; // 슬라임 소멸 후 시간 측정용

    // 슬라임 소멸 조건
    [SerializeField] private int lightSlimeDisappearThreshold = 99;      // 빛 슬라임 소멸 조건 (이하)
    [SerializeField] private int darkSlimeDisappearThreshold = 1;        // 어둠 슬라임 소멸 조건 (이상)
    [SerializeField] private int waterSlimeDisappearThreshold = 90;      // 물 슬라임 소멸 조건 (이하)
    [SerializeField] private int iceSlimeHumidityThreshold = 90;         // 얼음 슬라임 습도 소멸 조건 (이하)
    [SerializeField] private int iceSlimeTemperatureThreshold = -9;      // 얼음 슬라임 온도 소멸 조건 (이상)
    [SerializeField] private int fireSlimeTemperatureThreshold = 49;     // 불 슬라임 온도 소멸 조건 (이하)
    [SerializeField] private int plantSlimeLightThreshold = 40;          // 식물 슬라임 조명 소멸 조건 (이하)
    [SerializeField] private int plantSlimeHumidityThreshold = 10;       // 식물 슬라임 습도 소멸 조건 (이하)

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

        // 게임 시작 시 첫 슬라임 생성
        CreateSlime(SlimeType.Normal);
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
            if (previousSlimeType == slimeType)
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
            if (slimeType == SlimeType.Normal)
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

    public void CreateSlime(SlimeType slimeType)
    {
        // 슬라임 생성
        currentSlime = Instantiate(slimePrefab, new Vector3(-0.62f, 0.5f, -0.65f), Quaternion.identity);
        
        if (gameManager.isFirstStart)
        {
            type = (int)SlimeType.Normal; // 게임이 처음 시작되었을 때는 기본 슬라임 생성
            gameManager.isFirstStart = false; // 첫 시작 플래그 해제
        }
        else
        {
            choiceUiObject.SetActive(true);
            type = (int)slimeType;
        }

        // 슬라임 데이터 가져오기
        slimeData = DataTableManager.SlimeTable.Get(DataTableIds.SlimeIds[type]);

        if (slimeData != null)
        {
            CurrentSlimeId = slimeData.SlimeId;     // 현재 슬라임 ID 저장

            // 문자열 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(slimeData.SlimeName);
            var stringScriptsData = DataTableManager.StringTable.Get(slimeData.SlimeScript);
            if (stringData != null || stringScriptsData != null)
            {
                StringScripts = stringScriptsData.Value; // 현재 슬라임 스크립트 저장
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

    // 현재 슬라임 타입 반환
    public SlimeType GetCurrentSlimeType()
    {
        return (SlimeType)type;
    }

    // 슬라임을 강제로 소멸시키는 메서드 (환경 조건 불만족 시)
    public void ForceDisappear(string reason = "환경 조건 불만족")
    {
        if (currentSlime != null)
        {
            Destroy(currentSlime);
            SlimeDestroyed = true;
        }
    }

    // 현재 환경에서 슬라임이 소멸해야 하는지 확인
    public bool ShouldDisappearInCurrentEnvironment(EnvironmentManager environmentManager)
    {
        if (environmentManager == null || !HasCurrentSlime())
        {
            return false;
        }

        SlimeType currentType = GetCurrentSlimeType();
        
        // 현재 환경 상태
        int lightStep = environmentManager.LightStep;
        int lightValue = lightStep * 5; // 단계를 실제 밝기 값으로 변환
        int humidity = environmentManager.Humidity;
        int temperature = environmentManager.AirconTemp + (environmentManager.StoveStep * 10);
        bool hasFlowerPot = environmentManager.IsFlower;

        Debug.Log($"소멸 체크 - 슬라임: {currentType}, 조명: {lightValue}, 습도: {humidity}%, 온도: {temperature}°C, 화분: {hasFlowerPot}");

        // CSV 데이터 기반 소멸 조건 체크
        switch (currentType)
        {
            case SlimeType.Normal:
                // 기본 슬라임은 소멸하지 않음
                return false;

            case SlimeType.Light:
                // 빛 슬라임: 조명 밝기 임계값 이하로 떨어지면 소멸
                if (lightValue <= lightSlimeDisappearThreshold)
                {
                    Debug.Log($"빛 슬라임 소멸 조건 만족: 조명 밝기 {lightSlimeDisappearThreshold} 이하");
                    return true;
                }
                break;

            case SlimeType.Dark:
                // 어둠 슬라임: 조명 밝기 임계값 이상이면 소멸
                if (lightValue >= darkSlimeDisappearThreshold)
                {
                    Debug.Log($"어둠 슬라임 소멸 조건 만족: 조명 밝기 {darkSlimeDisappearThreshold} 이상");
                    return true;
                }
                break;

            case SlimeType.Water:
                // 물 슬라임: 습도 임계값 이하로 떨어지면 소멸
                if (humidity <= waterSlimeDisappearThreshold)
                {
                    Debug.Log($"물 슬라임 소멸 조건 만족: 습도 {waterSlimeDisappearThreshold}% 이하");
                    return true;
                }
                break;

            case SlimeType.Ice:
                // 얼음 슬라임: 습도 임계값 이하 또는 온도 임계값 이상이면 소멸
                if (humidity <= iceSlimeHumidityThreshold || temperature >= iceSlimeTemperatureThreshold)
                {
                    Debug.Log($"얼음 슬라임 소멸 조건 만족: 습도 {humidity}% ({iceSlimeHumidityThreshold}% 이하) 또는 온도 {temperature}°C ({iceSlimeTemperatureThreshold}°C 이상)");
                    return true;
                }
                break;

            case SlimeType.Fire:
                // 불 슬라임: 온도 임계값 이하일 때 소멸
                if (temperature <= fireSlimeTemperatureThreshold)
                {
                    Debug.Log($"불 슬라임 소멸 조건 만족: 온도 {fireSlimeTemperatureThreshold}°C 이하");
                    return true;
                }
                break;

            case SlimeType.Plant:
                // 식물 슬라임: 화분 제거 또는 조명 임계값 이하 또는 습도 임계값 이하일 때 소멸
                if (!hasFlowerPot || lightValue <= plantSlimeLightThreshold || humidity <= plantSlimeHumidityThreshold)
                {
                    Debug.Log($"식물 슬라임 소멸 조건 만족: 화분 {hasFlowerPot}, 조명 {lightValue} ({plantSlimeLightThreshold} 이하), 습도 {humidity}% ({plantSlimeHumidityThreshold}% 이하)");
                    return true;
                }
                break;
        }

        return false;
    }
}
