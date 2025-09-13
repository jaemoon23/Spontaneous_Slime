using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlimeManager slime; // 슬라임 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    private EnvironmentManager environmentManager;
    private GameObject environmentManagerObject;
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
    public SlimeType GetSlimeTypeByEnvironment()
    {
        if (environmentManager == null)
        {
            Debug.LogWarning("EnvironmentManager를 찾을 수 없습니다. Normal 슬라임을 반환합니다.");
            return SlimeType.Normal;
        }

        // 현재 환경 상태
        int lightStep = environmentManager.LightStep;
        int lightValue = lightStep * 5; // 단계를 실제 밝기 값으로 변환 (추정)
        int humidity = environmentManager.Humidity;
        int temperature = environmentManager.AirconTemp + (environmentManager.StoveStep * 10); // 에어컨 + 난로 온도
        bool hasFlowerPot = environmentManager.IsFlower;

        Debug.Log($"현재 환경 - 조명: {lightValue}, 습도: {humidity}%, 온도: {temperature}°C, 화분: {hasFlowerPot}");

        // 우선순위에 따른 슬라임 타입 결정 (CSV 데이터 기반)

        // 1. 식물 슬라임 (Plant) - 화분 + 조명 70 + 습도 50
        if (hasFlowerPot && lightValue >= 70 && humidity >= 50)
        {
            Debug.Log("식물 슬라임 조건 만족");
            return SlimeType.Plant;
        }

        // 2. 불 슬라임 (Fire) - 온도 50°C 이상
        if (temperature >= 50)
        {
            Debug.Log("불 슬라임 조건 만족");
            return SlimeType.Fire;
        }

        // 3. 얼음 슬라임 (Ice) - 습도 100% + 온도 -10°C 이하
        if (humidity >= 100 && temperature <= -10)
        {
            Debug.Log("얼음 슬라임 조건 만족");
            return SlimeType.Ice;
        }

        // 4. 물 슬라임 (Water) - 습도 100%
        if (humidity >= 100)
        {
            Debug.Log("물 슬라임 조건 만족");
            return SlimeType.Water;
        }

        // 5. 빛 슬라임 (Light) - 조명 밝기 100 (20단계)
        if (lightValue >= 100)
        {
            Debug.Log("빛 슬라임 조건 만족");
            return SlimeType.Light;
        }

        // 6. 어둠 슬라임 (Dark) - 조명 밝기 0
        if (lightValue <= 0)
        {
            Debug.Log("어둠 슬라임 조건 만족");
            return SlimeType.Dark;
        }

        // 기본값: 일반 슬라임
        Debug.Log("기본 슬라임 조건");
        return SlimeType.Normal;
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