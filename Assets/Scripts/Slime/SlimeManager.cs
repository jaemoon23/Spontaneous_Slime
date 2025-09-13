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
    private string expressions; // 표정
    public string stringScripts { get; private set; }   // 대사
    private string stringScriptsId;   // 대사
    public string SlimeNameId { get; private set; } // 슬라임 이름 ID
    public string SlimeName { get; private set; } // 슬라임 이름

    private Sprite icon;    // 아이콘
    public string GiftId { get; private set; }
    [SerializeField] private SlimeType slimeType = SlimeType.Normal;
    [SerializeField] private int type = 0; // 슬라임 타입 설정용
    public bool SlimeDestroyed { get; private set; } = false;
    private GameObject slimePrefab;
    private GameObject currentSlime; // 현재 생성된 슬라임 오브젝트
    private SlimeGrowth slimeGrowth;
    private float time = 0f; // 슬라임 소멸 후 시간 측정용

    GameManager gameManager;
    GameObject gameManagerObject;
    private GameObject slimeManager; // 슬라임 매니저 오브젝트 참조

    private Reward reward;
    private void Awake()
    {
        SlimeDestroyed = false;
    }

    private void Start()
    {
        gameManagerObject = GameObject.FindWithTag(Tags.GameManager);
        gameManager = gameManagerObject.GetComponent<GameManager>();

        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        reward = slimeManager.GetComponent<Reward>();

        // 슬라임 프리팹 로드
        slimePrefab = Resources.Load<GameObject>(Paths.Slime);
        if (slimePrefab == null)
        {
            Debug.LogError("슬라임 프리팹을 로드할 수 없습니다!");
            return;
        }

        // 게임 시작 시 첫 슬라임 생성
        CreateSlime();
    }

    private void Update()
    {
        // 슬라임 성장 로직
        if (!SlimeDestroyed)
        {
            slimeGrowth = currentSlime.GetComponent<SlimeGrowth>();
            if (slimeGrowth != null && slimeGrowth.Level >= slimeGrowth.MaxLevel)
            {
                // 보상 지급
                if (reward != null)
                {
                    reward.GiveReward(GiftId);
                }
                else
                {
                    Debug.LogWarning("Reward 컴포넌트를 찾을 수 없습니다.");
                }

                // 도감에 추가하고 슬라임 제거
                AddToCollection();
                Destroy(currentSlime);
                SlimeDestroyed = true;
            }
        }
        if (SlimeDestroyed)
        {
            // 슬라임 소멸 후 일정 시간 후 재생성
            RespawnSlime();
        }
    }
    public void RespawnSlime()
    {
        time += Time.deltaTime;
        if (time > 3f) // currentSlime == null 조건 제거
        {
            // 환경 조건에 맞춰서 슬라임 생성
            CreateSlime(gameManager.GetSlimeTypeByEnvironment());
            time = 0f;
            SlimeDestroyed = false;
        }
    }
    public void CreateSlime(SlimeType slimeType = SlimeType.Normal)
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
            type = (int)slimeType;
        }

        // 슬라임 데이터 가져오기
        var slimeData = DataTableManager.SlimeTable.Get(DataTableIds.SlimeIds[type]);
        Debug.Log($"슬라임 타입: {slimeType}, 데이터 ID: {type}");

        if (slimeData != null)
        {
            SlimeNameId = slimeData.SlimeName; // 슬라임 이름 ID
            GiftId = slimeData.GiftItemId;     // 선물 아이템 ID
            stringScriptsId = slimeData.SlimeScript; // 대사 ID
            expressions = slimeData.SlimeExpression; // 표정 ID

            // 문자열 데이터 가져오기
            var stringData = DataTableManager.StringTable.Get(SlimeNameId);
            var stringScriptsData = DataTableManager.StringTable.Get(stringScriptsId);
            if (stringData != null || stringScriptsData != null)
            {
                SlimeName = stringData.Value;            // 슬라임 이름
                stringScripts = stringScriptsData.Value; // 대사 ID
                Debug.Log($"슬라임 이름: {SlimeName}");
            }

            Debug.Log($"슬라임 데이터 로드 완료: {SlimeNameId}, 선물 아이템 ID: {GiftId}");
            Debug.Log($"표정: {expressions}, 스크립트: {stringScripts}");
        }
    }

    // 도감에 슬라임 추가
    public void AddToCollection()
    {
        Debug.Log($"슬라임 {SlimeName}이 도감에 추가되었습니다.");
        // TODO: 도감에 추가하는 로직 구현
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
            Debug.Log($"슬라임 강제 소멸: {reason}");
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
                // 빛 슬라임: 조명 밝기 99 이하로 떨어지면 소멸
                if (lightValue <= 99)
                {
                    Debug.Log("빛 슬라임 소멸 조건 만족: 조명 밝기 99 이하");
                    return true;
                }
                break;

            case SlimeType.Dark:
                // 어둠 슬라임: 조명 밝기 1 이상이면 소멸
                if (lightValue >= 1)
                {
                    Debug.Log("어둠 슬라임 소멸 조건 만족: 조명 밝기 1 이상");
                    return true;
                }
                break;

            case SlimeType.Water:
                // 물 슬라임: 습도 90% 이하로 떨어지면 소멸
                if (humidity <= 90)
                {
                    Debug.Log("물 슬라임 소멸 조건 만족: 습도 90% 이하");
                    return true;
                }
                break;

            case SlimeType.Ice:
                // 얼음 슬라임: 습도 90% 이하 또는 온도 -9°C 이상이면 소멸
                if (humidity <= 90 || temperature >= -9)
                {
                    Debug.Log($"얼음 슬라임 소멸 조건 만족: 습도 {humidity}% (90% 이하) 또는 온도 {temperature}°C (-9°C 이상)");
                    return true;
                }
                break;

            case SlimeType.Fire:
                // 불 슬라임: 온도 49°C 이하일 때 소멸
                if (temperature <= 49)
                {
                    Debug.Log("불 슬라임 소멸 조건 만족: 온도 49°C 이하");
                    return true;
                }
                break;

            case SlimeType.Plant:
                // 식물 슬라임: 화분 제거 또는 조명 40 이하 또는 습도 10% 이하일 때 소멸
                if (!hasFlowerPot || lightValue <= 40 || humidity <= 10)
                {
                    Debug.Log($"식물 슬라임 소멸 조건 만족: 화분 {hasFlowerPot}, 조명 {lightValue} (40 이하), 습도 {humidity}% (10% 이하)");
                    return true;
                }
                break;
        }

        return false;
    }
}
