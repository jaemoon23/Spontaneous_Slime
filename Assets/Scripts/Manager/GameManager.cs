using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lightStepToValueMultiplier = 5; // 조명 단계를 밝기 값으로 변환하는 배수
    private int stoveStepToTempMultiplier = 10; // 난로 단계를 온도로 변환하는 배수

    [Header("슬라임 스폰 조건")]
    [SerializeField] private int plantSlimeLightThreshold = 70; // 식물 슬라임 조명 조건
    [SerializeField] private int plantSlimeHumidityThreshold = 50; // 식물 슬라임 습도 조건
    [SerializeField] private int fireSlimeTempThreshold = 50; // 불 슬라임 온도 조건
    [SerializeField] private int iceSlimeHumidityThreshold = 100; // 얼음 슬라임 습도 조건
    [SerializeField] private int iceSlimeTempThreshold = -10; // 얼음 슬라임 온도 조건
    [SerializeField] private int waterSlimeHumidityThreshold = 100; // 물 슬라임 습도 조건
    [SerializeField] private int lightSlimeLightThreshold = 100; // 빛 슬라임 조명 조건
    [SerializeField] private int darkSlimeLightThreshold = 0; // 어둠 슬라임 조명 조건

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
        int lightStep = environmentManager.LightStep;   // 조명 단계 0~20
        int lightValue = lightStep * lightStepToValueMultiplier; // 단계를 실제 밝기 값으로 변환
        int humidity = environmentManager.Humidity; // 습도
        int temperature = environmentManager.AirconTemp + (environmentManager.StoveStep * stoveStepToTempMultiplier); // 에어컨 + 난로 온도
        bool hasFlowerPot = environmentManager.IsFlower; // 화분 존재 여부


        // 우선순위에 따른 슬라임 타입 결정

        // 1. 식물 슬라임 (Plant)
        if (hasFlowerPot && lightValue >= plantSlimeLightThreshold && humidity >= plantSlimeHumidityThreshold)
        {
            Debug.Log("식물 슬라임 조건 만족");

            slime.slimeType = SlimeType.Plant;
        }

        // 2. 불 슬라임 (Fire)
        if (temperature >= fireSlimeTempThreshold)
        {
            Debug.Log("불 슬라임 조건 만족");
            slime.slimeType = SlimeType.Fire;
        }

        // 3. 얼음 슬라임 (Ice)
        if (humidity >= iceSlimeHumidityThreshold && temperature <= iceSlimeTempThreshold)
        {
            Debug.Log("얼음 슬라임 조건 만족");
            slime.slimeType = SlimeType.Ice;
        }

        // 4. 물 슬라임 (Water)
        if (humidity >= waterSlimeHumidityThreshold)
        {
            Debug.Log("물 슬라임 조건 만족");
            slime.slimeType = SlimeType.Water;
        }

        // 5. 빛 슬라임 (Light)
        if (lightValue >= lightSlimeLightThreshold)
        {
            Debug.Log("빛 슬라임 조건 만족");
            slime.slimeType = SlimeType.Light;
        }

        // 6. 어둠 슬라임 (Dark) 
        if (lightValue <= darkSlimeLightThreshold)
        {
            Debug.Log("어둠 슬라임 조건 만족");
            slime.slimeType = SlimeType.Dark;
        }

        // 기본값: 일반 슬라임
        // Debug.Log("기본 슬라임 조건");
        // return SlimeType.Normal;
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