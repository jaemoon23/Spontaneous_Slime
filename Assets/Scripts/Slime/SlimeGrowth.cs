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

    public bool IsStartCoroutine = false;
    private int index = 0;
    public int Index { get { return index; } set { index = value; } }
    [SerializeField] private int expPerTouch = 1;
    public int CurrentExp { get; set; } = 0;
    public int Level { get; set; }
    public int MaxLevel { get; private set; } = 10;
    private bool isMaxLevel = false;
    public int MaxExp { get; set; }
    



    public int PreviousScaleLevel { get; set; } // 이전 스케일
    public int ScaleLevel { get; set; }
    private Vector3 baseScale;
    private Reward reward;
    private SlimeManager slimeManager;
    private GameObject slimeManagerObject; // 슬라임 매니저 오브젝트 참조
    private UiManager uiManager;
    private GameObject uiManagerObject;
    private Coroutine scalingCoroutine;
    private LevelUpData1 levelData;
    private float timer = 0;
    private float interval = 0.3f; // 1.2초 간격

    

    private void Awake()
    {
        IsStartCoroutine = false;
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
            CurrentExp = MaxExp;
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            uiManager.ShowMaxLevelPanel();
            return;
        }


        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
            scalingCoroutine = null;
        }

        if (ScaleLevel != PreviousScaleLevel)
        {
            IsStartCoroutine = true;
            scalingCoroutine = StartCoroutine(CoScaleUp(1f));
            if (MaxLevel <= Level)
            {
                // 1초 대기 후 슬라임 파괴
                StartCoroutine(CoDestroySlimeDelay(1f));
            }
        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);
    }

    public void OnTouch()
    {
        // if (timer < interval)
        // {
        //     Debug.Log($"터치 딜레이 중... 남은 시간: {interval - timer:F1}초");
        //     return;
        // }
        // timer = 0f;

        slimeManager.ChangeSlimeExpression();
        uiManager.ShowScriptWindow();


        if (IsStartCoroutine)
        {
            return;
        }

        // 맥스레벨에 도달한 경우 경험치 처리
        if (isMaxLevel)
        {
            CurrentExp = MaxExp; // 경험치를 최대치로 고정
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            Debug.Log($"맥스레벨 도달! 경험치: {CurrentExp} / {MaxExp}");
            return;
        }

        CurrentExp += expPerTouch;
        if (CurrentExp >= MaxExp)
        {
            LevelUp();
        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
    }

    // 치트용 메서드: 딜레이 없이 직접 경험치 증가
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
    private IEnumerator CoScaleUp(float duration)
    {

        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(ScaleLevel, ScaleLevel, ScaleLevel);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        transform.localScale = endScale;
        IsStartCoroutine = false;
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
            slimeManager.DestroySlime();
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
            CurrentExp -= MaxExp;
            MaxExp = data.NeedExp;
            PreviousScaleLevel = ScaleLevel;
            ScaleLevel = data.ScaleLevel;

    
            if (Level >= MaxLevel)
            {
                isMaxLevel = true;
                CurrentExp = MaxExp; // 맥스레벨이면 경험치 풀로 채우기
                Debug.Log($"초기 로드: 맥스레벨 {MaxLevel} 상태, 경험치 최대로 설정");
            }
        }
        else
        {
            Debug.LogError("레벨업 데이터를 찾을 수 없습니다!");
        }
    }
}
