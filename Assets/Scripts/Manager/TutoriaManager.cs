using UnityEngine;
using Excellcube.EasyTutorial.Utils;
public class TutoriaManager : MonoBehaviour
{
    [SerializeField] private SlimeManager slimeManager;      // 인스펙터로 할당
    [SerializeField] private GameManager gameManager;        // 게임매니저 참조 추가
    public SlimeType slimeType = SlimeType.Normal; // 원하는 타입 지정

    [SerializeField] private GameObject LeftImage;
    [SerializeField] private GameObject RightImage;
    [SerializeField] private GameObject dialoguePanel;

    private bool IsTutorialActive = true;
    private bool waitingForLevel2 = false; // 레벨 2 대기 상태
    private bool waitingForLevel10 = false; // 레벨 10 대기 상태

    private void OnEnable()
    {
        // 슬라임 레벨 변경 이벤트 구독
        SlimeGrowth.OnLevelChanged += OnSlimeLevelChanged;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        SlimeGrowth.OnLevelChanged -= OnSlimeLevelChanged;
    }

    // 슬라임 레벨이 변경될 때 호출되는 메서드
    private void OnSlimeLevelChanged(int newLevel)
    {
        Debug.Log($"[TutorialManager] 슬라임 레벨 변경: {newLevel}");
        
        if (waitingForLevel2 && newLevel >= 2)
        {
            Debug.Log("[TutorialManager] 레벨 2 달성!");
            waitingForLevel2 = false;
            OnSlimeReachedLevel2();
        }
        if (waitingForLevel10 && newLevel >= 10)
        {
            Debug.Log("[TutorialManager] 레벨 10 달성!");
            waitingForLevel10 = false;
            OnSlimeReachedLevel10();
        }
    }
    

    // On Tutorial Begin 연결
    public void SpawnSlime()
    {
        if (slimeManager != null)
        {
            // 튜토리얼에서 생성하므로 첫 시작 플래그 해제
            if (gameManager != null)
            {
                gameManager.isFirstStart = false;
            }
            ActiveTutorial();
            // 슬라임 생성 (선택 UI 표시 안함, 스폰 텍스트 표시)
            slimeManager.CreateSlime(slimeType, false, true, false);
            CurrencyManager.Instance.AddEther(20); // 튜토리얼용 골드 추가
            
            Debug.Log("튜토리얼에서 첫 슬라임 생성 완료!");
        }
    }
    public void ActiveTutorial()
    {
        IsTutorialActive = !IsTutorialActive;
        LeftImage.SetActive(IsTutorialActive);
        RightImage.SetActive(IsTutorialActive);
        dialoguePanel.SetActive(IsTutorialActive);
    }

    // 첫 시작 여부 확인
    public bool IsFirstStart()
    {
        return gameManager != null && gameManager.isFirstStart;
    }

    public void NomalSlimeLevelUp()
    {
        // 현재 슬라임이 있는지 확인
        if (slimeManager != null && slimeManager.HasCurrentSlime())
        {
            var currentSlime = slimeManager.GetCurrentSlime();
            var slimeGrowth = currentSlime?.GetComponent<SlimeGrowth>();
            
            if (slimeGrowth != null)
            {
                // 현재 레벨이 2 미만일 때만 레벨업 처리
                if (slimeGrowth.Level < 2)
                {
                    Debug.Log("튜토리얼: 슬라임 레벨 2 달성을 기다리는 중...");
                    waitingForLevel2 = true; // 레벨 2 대기 상태 활성화
                }
                else
                {
                    Debug.Log("슬라임이 이미 레벨 2 이상입니다.");
                    OnSlimeReachedLevel2(); // 이미 조건을 만족하므로 다음 단계로
                }
            }
        }
        else
        {
            Debug.LogWarning("현재 슬라임이 없습니다!");
        }
    }
    public void NormalSlimeLevelMax()
    {
        Debug.Log("[TutorialManager] NormalSlimeLevelMax 메서드 호출됨!");
        
        // 현재 슬라임이 있는지 확인
        if (slimeManager != null && slimeManager.HasCurrentSlime())
        {
            var currentSlime = slimeManager.GetCurrentSlime();
            var slimeGrowth = currentSlime?.GetComponent<SlimeGrowth>();
            
            if (slimeGrowth != null)
            {
                Debug.Log($"[TutorialManager] 현재 슬라임 레벨: {slimeGrowth.Level}, waitingForLevel10: {waitingForLevel10}");
                
                // 현재 레벨이 10 미만일 때만 대기 상태 활성화
                if (slimeGrowth.Level < 10)
                {
                    Debug.Log("튜토리얼: 슬라임 레벨 10 달성을 기다리는 중...");
                    waitingForLevel10 = true; // 레벨 10 대기 상태 활성화
                }
                else
                {
                    Debug.Log("슬라임이 이미 레벨 10 이상입니다.");
                    OnSlimeReachedLevel10(); // 이미 조건을 만족하므로 다음 단계로
                }
            }
        }
        else
        {
            Debug.LogWarning("현재 슬라임이 없습니다!");
        }
    }
    
    // 슬라임이 레벨 2에 도달했을 때 호출되는 메서드
    private void OnSlimeReachedLevel2()
    {
        Debug.Log("튜토리얼: 슬라임 레벨 2 달성! 다음 단계로 진행합니다.");
        
        // EasyTutorial 스타일로 이벤트 브로드캐스트

        
        
        // 또는 직접 다음 튜토리얼 단계 실행
        StartNextTutorialStep();
    }
    private void OnSlimeReachedLevel10()
    {
        Debug.Log("튜토리얼: 슬라임 레벨 10 달성! 다음 단계로 진행합니다.");

        //다음 튜토리얼 단계 실행
        StartNextTutorialStep1();
    }

    // 다음 튜토리얼 단계 실행
    private void StartNextTutorialStep()
    {
        // 여기에 레벨 2 달성 후 실행할 튜토리얼 로직 추가
        Debug.Log("다음 튜토리얼 단계를 시작합니다!");



        // 여기서 다음 튜토리얼 페이지로 이동
        TutorialEvent.Instance.Broadcast("TUTORIAL_LEVEL_UP_COMPLETE");
    }
    private void StartNextTutorialStep1()
    {
        // 여기에 레벨 10 달성 후 실행할 튜토리얼 로직 추가
        Debug.Log("다음 튜토리얼 단계를 시작합니다!");



        // 여기서 다음 튜토리얼 페이지로 이동
        TutorialEvent.Instance.Broadcast("TUTORIAL_MAX_LEVEL_UP_COMPLETE");
    }
}
