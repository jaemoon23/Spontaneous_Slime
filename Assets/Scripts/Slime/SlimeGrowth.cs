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

    public bool IsStartCoroutine = false;
    private int index = 0;
    [SerializeField] private int expPerTouch = 1;
    private int currentExp = 0;
    public int Level { get; private set; }
    public int MaxLevel { get; private set; } = 10;
    private int maxExp;

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
        currentExp = 0;
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
            maxExp = levelData.NeedExp;
            scaleLevel = levelData.ScaleLevel;
        }
        else
        {
            Debug.LogError("레벨업 데이터를 찾을 수 없습니다!");
        }
        
        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(currentExp, maxExp);
        OnLevelChanged?.Invoke(Level);
    }
    private void Update()
    {
        // 1 키를 눌렀을 때 레벨 1씩 증가
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!IsStartCoroutine) // 레벨업 중이 아닐 때만 실행
            {
                IsStartCoroutine = true;
                LevelUp();
                Debug.Log($"1 키 입력: 레벨 1 증가\n현재 레벨: {Level}");
            }
        }
    }

    private void LevelUp()
    {
        if (index >= DataTableIds.LevelUpIds.Length - 1)
        {
            return;
        }

        index++;
        // 레벨 데이터 직접 접근
        var levelData = DataTableManager.LevelUpTable.Get(DataTableIds.LevelUpIds[index]);
        if (levelData != null)
        {
            Level = levelData.CurrentLevel;
            currentExp -= maxExp;
            maxExp = levelData.NeedExp;
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
                scalingCoroutine = StartCoroutine(CoScaleUpDown(1f));
            }

        }

        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(currentExp, maxExp);
        OnLevelChanged?.Invoke(Level);

    }

    public void OnTouch()
    {
        uiManager.ShowScriptWindow();
        if (IsStartCoroutine)
        {
            return;
        }
        currentExp += expPerTouch;
        if (currentExp >= maxExp)
        {
            LevelUp();
        }
        
        // UI 업데이트 이벤트 발생
        OnExpChanged?.Invoke(currentExp, maxExp);
        
        Debug.Log($"경험치 :{currentExp} / {maxExp}");
        Debug.Log($"레벨 :{Level}");
    }

    private IEnumerator CoScaleUpDown(float duration)
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

    
}
