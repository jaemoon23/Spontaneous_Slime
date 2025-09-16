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
    [SerializeField] private int expPerTouch = 1;
    public int CurrentExp { get; private set; } = 0;
    public int Level { get; private set; }
    public int MaxLevel { get; private set; } = 10;
    private bool isMaxLevel = false;
    public int MaxExp { get; private set; }

    private int previousScaleLevel; // 이전 스케일
    private int scaleLevel;
    private Vector3 baseScale;
    private Reward reward;
    private SlimeManager slime;
    private GameObject slimeManager; // 슬라임 매니저 오브젝트 참조
    private UiManager uiManager;
    private GameObject uiManagerObject;
    private Coroutine scalingCoroutine;

    private void Awake()
    {
        IsStartCoroutine = false;
        index = 0;
        CurrentExp = 0;
    }

    private void Start()
    {
        baseScale = transform.localScale;
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();
        reward = slimeManager.GetComponent<Reward>();


        uiManagerObject = GameObject.FindWithTag(Tags.UiManager);
        uiManager = uiManagerObject.GetComponent<UiManager>();

        // 초기 레벨 데이터 직접 접근
        var levelData = DataTableManager.LevelUpTable.Get(DataTableIds.LevelUpIds[index]);
        if (levelData != null)
        {
            Level = levelData.CurrentLevel;
            MaxExp = levelData.NeedExp;
            scaleLevel = levelData.ScaleLevel;
            
            // 초기 로드 시에도 맥스레벨 체크
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
        
        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);
    }

    public void LevelUp()
    {
        index++;
        if (index >= DataTableIds.LevelUpIds.Length - 1)
        {
            index = DataTableIds.LevelUpIds.Length - 1;
            Debug.Log("더 이상 레벨업할 수 없습니다. 최대 레벨에 도달했습니다.");
        }

        // 레벨 데이터 직접 접근
        var levelData = DataTableManager.LevelUpTable.Get(DataTableIds.LevelUpIds[index]);
        if (levelData != null)
        {
            
            // 새로 레벨업한 후 맥스레벨 체크
            if (Level >= MaxLevel && CurrentExp >= MaxExp)
            {
                CurrentExp = MaxExp;
                OnExpChanged?.Invoke(CurrentExp, MaxExp);
                
                uiManager.ShowMaxLevelPanel();
                
                return;
            }
            Level = levelData.CurrentLevel;
            CurrentExp -= MaxExp;
            MaxExp = levelData.NeedExp; 
            
            previousScaleLevel = scaleLevel;
            scaleLevel = levelData.ScaleLevel;
            
            if (scalingCoroutine != null)
            {
                StopCoroutine(scalingCoroutine);
                scalingCoroutine = null;
            }
            if (scaleLevel != previousScaleLevel)
            {
                IsStartCoroutine = true;
                scalingCoroutine = StartCoroutine(CoScaleUp(1f));
                if (MaxLevel <= Level)
                {
                    // 1초 대기 후 슬라임 파괴
                    StartCoroutine(CoDestroySlimeDelay(1f));
                }
            }

        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);

    }

    public void OnTouch()
    {
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


    private IEnumerator CoScaleUp(float duration)
    {
        
        Vector3 startScale = transform.localScale;
        Vector3 endScale = baseScale * scaleLevel;
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
        slime.DestroySlime();
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
            slime.DestroySlime();
        }
    }

    
}
