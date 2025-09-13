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
        CreateSlime(slimeType);
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
            //TDO: 환경 조건에 맞춰서 슬라임 생성
            CreateSlime(SlimeType.Dark);
            time = 0f;
            SlimeDestroyed = false;
        }
    }
    public void CreateSlime(SlimeType slimeType = SlimeType.Normal)
    {
        // 슬라임 생성
        currentSlime = Instantiate(slimePrefab, new Vector3(-0.62f, 0.5f, -0.65f), Quaternion.identity);
#if UNITY_ANDROID || UNITY_IOS    
        if (gameManager.isFirstStart)
        {
            type = (int)SlimeType.Normal; // 게임이 처음 시작되었을 때는 기본 슬라임 생성
            gameManager.isFirstStart = false; // 첫 시작 플래그 해제
        }
        else
        {
            type = (int)slimeType;
        }
#endif
#if UNITY_EDITOR
        type = (int)slimeType;
#endif
        // 슬라임 데이터 가져오기
        var slimeData = DataTableManager.SlimeTable.Get(DataTableIds.SlimeIds[type]);
        Debug.Log($"슬라임 타입: {slimeType}, 데이터 ID: {type}");
        // TODO: 환경 조건에 맞춰서 슬라임 생성 / 게임이 처음 시작되었을 때 기본 슬라임 생성


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

    public void AddToCollection()
    {
        Debug.Log($"슬라임 {SlimeName}이 도감에 추가되었습니다.");
        // TODO: 도감에 추가하는 로직 구현
    }
}
