using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
        }
        else
        {
            Debug.LogError("레벨업 데이터를 찾을 수 없습니다!");
        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(Level);
    }
    private void Update()
    {
        // 1을 눌렀을때 1렙 증가
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentExp += MaxExp;
            if (CurrentExp >= MaxExp)
            {
                LevelUp();
                OnExpChanged?.Invoke(CurrentExp, MaxExp);
            }
            Debug.Log($"경험치 :{CurrentExp} / {MaxExp}");
            Debug.Log($"레벨 :{Level}");
        }

        // 스페이스 키를 눌렀을 때 경험치 1씩 증가 (테스트용)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 10; i++)
            {
                CurrentExp = MaxExp;
                OnExpChanged?.Invoke(CurrentExp, MaxExp);
                if (CurrentExp >= MaxExp)
                {
                    LevelUp();
                }
            }

            Debug.Log($"경험치 :{CurrentExp} / {MaxExp}");
            Debug.Log($"레벨 :{Level}");
        }
    }

    private void LevelUp()
    {
        index++;
        if (index >= DataTableIds.LevelUpIds.Length - 1)
        {
            index = DataTableIds.LevelUpIds.Length - 1;
        }
        
        if (Level >= MaxLevel)
        {
            Debug.Log("최대 레벨 조건 충족!");
            if (CurrentExp >= MaxExp)
            {
                CurrentExp = MaxExp;
                OnExpChanged?.Invoke(CurrentExp, MaxExp);
                OnSlimeMaxLevel?.Invoke();

                uiManager.ShowMaxLevelPanel();
                uiManager.UpdateMaxLevelText(Strings.MaxLevel);


                if (!uiManager.GetMaxLevel())
                {
                    uiManager.DisableExpUI(false);
                    slime.DestroySlime();
                    uiManager.IsMaxPanelActive = false;
                }
                return;
            }
        }

        var levelData = DataTableManager.LevelUpTable.Get(DataTableIds.LevelUpIds[index]);
        if (levelData != null)
        {
            Level = levelData.CurrentLevel;
            CurrentExp -= MaxExp;
            MaxExp = levelData.NeedExp;


            previousScaleLevel = scaleLevel;
            scaleLevel = levelData.ScaleLevel;

            Debug.Log($"레벨업 완료 - Level: {Level}, MaxLevel: {MaxLevel}, CurrentExp: {CurrentExp}, MaxExp: {MaxExp}");
            // 새로 레벨업한 후 맥스레벨 체크


            if (scalingCoroutine != null)
            {
                StopCoroutine(scalingCoroutine);
                scalingCoroutine = null;
            }
            if (scaleLevel != previousScaleLevel)
            {
                IsStartCoroutine = true;
                scalingCoroutine = StartCoroutine(CoScaleUp(1f));
            }

            // UI 업데이트 이벤트 발생
            OnExpChanged?.Invoke(CurrentExp, MaxExp);
            OnLevelChanged?.Invoke(Level);

        }
    }

    public void OnTouch()
    {
        uiManager.ShowScriptWindow();
        if (IsStartCoroutine)
        {
            return;
        }

        // 맥스레벨에 도달한 경우 경험치 처리
        // if (isMaxLevel)
        // {
        //     OnExpChanged?.Invoke(CurrentExp, MaxExp);
        //     Debug.Log($"맥스레벨 도달! 경험치: {CurrentExp} / {MaxExp}");
        //     return;
        // }

        CurrentExp += expPerTouch;
        // UI 업데이트 이벤트 발생
        if (CurrentExp >= MaxExp)
        {
            LevelUp();
        }
        OnExpChanged?.Invoke(CurrentExp, MaxExp);

        Debug.Log($"경험치 :{CurrentExp} / {MaxExp}");
        Debug.Log($"레벨 :{Level}");
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
        
    }

    
}
