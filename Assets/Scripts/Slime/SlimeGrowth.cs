using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SlimeGrowth : MonoBehaviour, ITouchable
{
    // 이벤트 선언
    public static event Action<int, int> OnExpChanged; // currentExp, maxExp
    public static event Action<int> OnLevelChanged; // level
    public static event Action OnSlimeMaxLevel; // 최대 레벨 도달 이벤트

    private bool isScaling = false; // 스케일링 중인지 확인하는 플래그
    private float scalingDuration = 1f; // 스케일링 지속 시간
    private float scalingTimer = 0f; // 스케일링 타이머
    private Vector3 startScale; // 스케일링 시작 스케일
    private Vector3 targetScale; // 목표 스케일
    private int index = 0;
    public int Index { get { return index; } set { index = value; } }
    [SerializeField] private int expPerTouch = 1;
    public int CurrentExp { get; set; } = 0;
    public int Level { get; set; }
    public int MaxLevel { get; private set; } = 10;
    public bool isMaxLevel { get; private set; } = false;
    public int MaxExp { get; set; }
    



    public int PreviousScaleLevel { get; set; } // 이전 스케일
    public int ScaleLevel { get; set; }
    private Vector3 baseScale;
    private Reward reward;
    private SlimeManager slimeManager;
    private GameObject slimeManagerObject; // 슬라임 매니저 오브젝트 참조
    private UiManager uiManager;
    private GameObject uiManagerObject;
    private LevelUpData1 levelData;
    private float timer = 0;
    private float interval = 0.3f; // 1.2초 간격

    private void Awake()
    {
        isScaling = false;
        index = 0;
        CurrentExp = 0;
    }

    private void Start()
    {
        baseScale = transform.localScale;

        slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        slimeManager = slimeManagerObject.GetComponent<SlimeManager>();

        reward = slimeManagerObject.GetComponent<Reward>();

        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        // 레벨 데이터 직접 접근 - 희귀도별 레벨업 테이블 사용
        var slimeData = DataTableManager.SlimeTable.Get(slimeManager.CurrentSlimeId);
        if (slimeData != null)
        {
            LoadLevelDataByRarity(slimeData.RarityId, index);
        }
        else
        {
            Debug.LogError("슬라임 데이터를 찾을 수 없습니다!");
        }


        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);

        
    }

    private void Update()
    {
        if (timer < interval)
        {
            timer += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        // Animator 업데이트 이후에 스케일 조정
        if (isScaling)
        {
            scalingTimer += Time.deltaTime;
            float t = scalingTimer / scalingDuration;
            
            if (t >= 1f)
            {
                // 스케일링 완료
                transform.localScale = targetScale;
                isScaling = false;
                scalingTimer = 0f;
            }
            else
            {
                // 스케일 보간
                Vector3 currentScale = Vector3.Lerp(startScale, targetScale, t);
                transform.localScale = currentScale;
            }
        }
    }
    public void ExpChanged()
    {
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
    }
    public void LevelChanged()
    {
        OnLevelChanged?.Invoke(Level);
    }

    public void LevelUp()
    {
        index++;

        // 현재 슬라임의 희귀도에 따른 최대 레벨 체크
        var slimeData = DataTableManager.SlimeTable.Get(slimeManager.CurrentSlimeId);
        if (slimeData == null)
        {
            Debug.LogError("슬라임 데이터를 찾을 수 없습니다!");
            return;
        }

        int[] currentLevelUpIds = GetLevelUpIdsByRarity(slimeData.RarityId);
        if (index >= currentLevelUpIds.Length - 1)
        {
            index = currentLevelUpIds.Length - 1;
            Debug.Log("더 이상 레벨업할 수 없습니다. 최대 레벨에 도달했습니다.");
        }

        // 희귀도별 레벨 데이터 로드
        LoadLevelDataByRarity(slimeData.RarityId, index);

        // 새로 레벨업한 후 맥스레벨 체크
        if (Level >= MaxLevel && CurrentExp >= MaxExp)
        {
            isMaxLevel = true;
            CurrentExp = MaxExp;
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            OnLevelChanged?.Invoke(Level);
            
            uiManager.ShowMaxLevelPanel();
            
            // 최대 레벨 도달 시에도 CSV의 LEVELUP_EHTER 값 사용
            var maxLevelSlimeData = DataTableManager.SlimeTable.Get(slimeManager.CurrentSlimeId);
            if (maxLevelSlimeData != null)
            {
                ILevelUpData maxLevelData = null;
                
                switch (maxLevelSlimeData.RarityId)
                {
                    case 1:
                        maxLevelData = DataTableManager.LevelUpTable1.Get(DataTableIds.LevelUpIds1[index]);
                        break;
                    case 2:
                        maxLevelData = DataTableManager.LevelUpTable2.Get(DataTableIds.LevelUpIds2[index]);
                        break;
                    case 3:
                        maxLevelData = DataTableManager.LevelUpTable3.Get(DataTableIds.LevelUpIds3[index]);
                        break;
                    case 4:
                        maxLevelData = DataTableManager.LevelUpTable4.Get(DataTableIds.LevelUpIds4[index]);
                        break;
                    case 5:
                        maxLevelData = DataTableManager.LevelUpTable5.Get(DataTableIds.LevelUpIds5[index]);
                        break;
                }
                
                if (maxLevelData != null)
                {
                    CurrencyManager.Instance.AddEther(maxLevelData.LevelUpEther);
                    Debug.Log($"최대 레벨 도달! {maxLevelData.LevelUpEther} 골드 획득");
                }
            }
            return;
        }

        // 기존 스케일링이 진행 중이면 중단 (연속 레벨업 대비)
        if (isScaling)
        {
            isScaling = false;
            scalingTimer = 0f;
        }

        // 모든 레벨업 시 CSV의 LEVELUP_EHTER 값에 따라 골드 지급
        var currentSlimeData = DataTableManager.SlimeTable.Get(slimeManager.CurrentSlimeId);
        if (currentSlimeData != null)
        {
            ILevelUpData currentLevelData = null;
            
            switch (currentSlimeData.RarityId)
            {
                case 1:
                    currentLevelData = DataTableManager.LevelUpTable1.Get(DataTableIds.LevelUpIds1[index]);
                    break;
                case 2:
                    currentLevelData = DataTableManager.LevelUpTable2.Get(DataTableIds.LevelUpIds2[index]);
                    break;
                case 3:
                    currentLevelData = DataTableManager.LevelUpTable3.Get(DataTableIds.LevelUpIds3[index]);
                    break;
                case 4:
                    currentLevelData = DataTableManager.LevelUpTable4.Get(DataTableIds.LevelUpIds4[index]);
                    break;
                case 5:
                    currentLevelData = DataTableManager.LevelUpTable5.Get(DataTableIds.LevelUpIds5[index]);
                    break;
            }
            
            if (currentLevelData != null)
            {
                CurrencyManager.Instance.AddEther(currentLevelData.LevelUpEther);
                Debug.Log($"레벨업! {currentLevelData.LevelUpEther} 에테르 획득");
            }
        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);
    }

    public void OnTouch()
    {

        slimeManager.ChangeSlimeExpression();
        uiManager.ShowScriptWindow();


        if (isScaling)
        {
            return;
        }

        // 맥스레벨에 도달한 경우 경험치 처리
        if (isMaxLevel)
        {
            return;
        }

        int initialScaleLevel = ScaleLevel; // 레벨업 전 스케일 저장
        
        CurrentExp += expPerTouch;
        if (Level >= MaxLevel && CurrentExp >= MaxExp)
        {
            CurrentExp = MaxExp;
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            uiManager.ShowMaxLevelPanel();
        }
        else if (CurrentExp >= MaxExp && Level < MaxLevel)
        {
            LevelUp();
        }
        
        // 레벨업으로 스케일이 변경되었다면 스케일링 시작
        if (ScaleLevel != initialScaleLevel)
        {
            StartScaling();
            
            if (MaxLevel <= Level)
            {
                // 1초 대기 후 슬라임 파괴
                StartCoroutine(CoDestroySlimeDelay(1f));
            }
        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
    }

    public void AddExp(int expAmount)
    {
        if (isMaxLevel)
        {
            CurrentExp = MaxExp;
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            return;
        }

        CurrentExp += expAmount;
        
        int initialScaleLevel = ScaleLevel; // 연속 레벨업 전 스케일 저장
        
        // 연속 레벨업 처리
        while (CurrentExp >= MaxExp && !isMaxLevel)
        {
            LevelUp();
            
            // 레벨업 후 최대 레벨에 도달했는지 확인
            if (isMaxLevel)
            {
                CurrentExp = MaxExp;
                break;
            }
        }
        
        // 연속 레벨업이 완료된 후 스케일이 변경되었다면 최종 스케일로 스케일링
        if (ScaleLevel != initialScaleLevel && !isMaxLevel)
        {
            StartScaling();
            
            if (MaxLevel <= Level)
            {
                // 1초 대기 후 슬라임 파괴
                StartCoroutine(CoDestroySlimeDelay(1f));
            }
        }

        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        Debug.Log($"경험치 {expAmount} 추가됨. 현재: {CurrentExp}/{MaxExp}");
    }

    // 치트용
    public void AddExpCheat(int expAmount = 1)
    {
        if (isMaxLevel)
        {
            CurrentExp = MaxExp;
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            return;
        }

        CurrentExp += expAmount;
        if (CurrentExp >= MaxExp)
        {
            LevelUp();
        }

        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        Debug.Log($"치트: 경험치 {expAmount} 추가됨. 현재: {CurrentExp}/{MaxExp}");
    }

    // 스케일링 시작 메서드
    private void StartScaling()
    {
        startScale = transform.localScale;
        targetScale = new Vector3(ScaleLevel, ScaleLevel, ScaleLevel);
        scalingTimer = 0f;
        isScaling = true;
        Debug.Log($"스케일링 시작: {startScale} > {targetScale}");
    }

    private IEnumerator CoDestroySlimeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Level = MaxLevel;
        uiManager.DisableExpUI(false);
        slimeManager.DestroySlime();
    }

    // MaxLevelPanel이 닫힐 때 호출되는 메서드
    public void SetMaxLevelState(bool state)
    {
        isMaxLevel = state;
        Debug.Log($"MaxLevel 상태 설정: {isMaxLevel}");
        
        if (isMaxLevel)
        {
            CurrentExp = MaxExp; // 최대 경험치로 설정
            OnSlimeMaxLevel?.Invoke(); // 최대 레벨 도달 이벤트 발생
            
            // 소환석 슬라임이었다면 소멸 후 자동 생성 로직 재개
            if (slimeManager.IsFromSummonStone)
            {
                Debug.Log("소환석 슬라임이 맥스레벨로 소멸 - 자동 생성 로직 재개");
                slimeManager.IsFromSummonStone = false; // 소환석 플래그 초기화
            }
            
            slimeManager.DestroySlime(); // 모든 슬라임이 맥스레벨에서 소멸 (기존 로직 유지)
        }
    }

    // 희귀도에 따라 레벨 데이터를 로드하는 메서드
    private void LoadLevelDataByRarity(int rarity, int levelIndex)
    {
        ILevelUpData levelData = null;
        
        switch (rarity)
        {
            case 1:
                levelData = DataTableManager.LevelUpTable1.Get(DataTableIds.LevelUpIds1[levelIndex]);
                break;
            case 2:
                levelData = DataTableManager.LevelUpTable2.Get(DataTableIds.LevelUpIds2[levelIndex]);
                break;
            case 3:
                levelData = DataTableManager.LevelUpTable3.Get(DataTableIds.LevelUpIds3[levelIndex]);
                break;
            case 4:
                levelData = DataTableManager.LevelUpTable4.Get(DataTableIds.LevelUpIds4[levelIndex]);
                break;
            case 5:
                levelData = DataTableManager.LevelUpTable5.Get(DataTableIds.LevelUpIds5[levelIndex]);
                break;
            default:
                Debug.LogError($"지원되지 않는 희귀도: {rarity}");
                break;
        }
        
        if (levelData != null)
        {
            SetLevelData(levelData);
        }
        else
        {
            Debug.LogError($"희귀도 {rarity}의 레벨 {levelIndex + 1} 데이터를 찾을 수 없습니다!");
        }
    }

    // 희귀도에 따라 LevelUpIds 배열을 반환하는 메서드
    private int[] GetLevelUpIdsByRarity(int rarity)
    {
        switch (rarity)
        {
            case 1:
                return DataTableIds.LevelUpIds1;
            case 2:
                return DataTableIds.LevelUpIds2;
            case 3:
                return DataTableIds.LevelUpIds3;
            case 4:
                return DataTableIds.LevelUpIds4;
            case 5:
                return DataTableIds.LevelUpIds5;
            default:
                Debug.LogError($"지원되지 않는 희귀도: {rarity}");
                return DataTableIds.LevelUpIds1; // 기본값
        }
    }
    public void SetLevelData(ILevelUpData data)
    {
        if (data != null)
        {
            Level = data.CurrentLevel;
            
            // 레벨업 시 남은 경험치 계산 (현재 경험치 - 이전 레벨의 최대 경험치)
            int remainingExp = CurrentExp - MaxExp;
            MaxExp = data.NeedExp;
            CurrentExp = remainingExp; // 남은 경험치를 새 레벨의 현재 경험치로 설정
            
            PreviousScaleLevel = ScaleLevel;
            ScaleLevel = data.ScaleLevel;

            if (Level >= MaxLevel && CurrentExp >= MaxExp)
            {
                isMaxLevel = true;
                Debug.Log($"초기 로드: 맥스레벨 {MaxLevel} 상태, 경험치 최대로 설정");
            }
            else
            {
                isMaxLevel = false;
                Debug.Log($"레벨업 완료! 레벨: {Level}, 남은 경험치: {CurrentExp}/{MaxExp}");
            }
        }
        else
        {
            Debug.LogError("레벨업 데이터를 찾을 수 없습니다!");
        }
    }
}
