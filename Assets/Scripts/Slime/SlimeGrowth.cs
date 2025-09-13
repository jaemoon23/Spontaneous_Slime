using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SlimeGrowth : MonoBehaviour, ITouchable
{
    public bool IsLevelUp { get; private set; } = false;
    private int index = 0;
    [SerializeField] private int expPerTouch = 1;
    private int currentExp = 0;
    public int Level { get; private set; }
    public int MaxLevel { get; private set; } = 10;
    private int maxExp;
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
        IsLevelUp = false;
        index = 0;
        currentExp = 0;
    }

    private void Start()
    {
        baseScale = transform.localScale;
        reward = GetComponent<Reward>();

        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();

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
    }
    private void Update()
    {
        // 1 키를 눌렀을 때 레벨 1씩 증가
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!IsLevelUp) // 레벨업 중이 아닐 때만 실행
            {
                IsLevelUp = true;
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
            scaleLevel = levelData.ScaleLevel;
            if (scalingCoroutine != null)
            {
                StopCoroutine(scalingCoroutine);
                scalingCoroutine = null;
            }
            scalingCoroutine = StartCoroutine(CoScaleUpDown(1f));
        }
    }

    public void OnTouch()
    {
        uiManager.ShowScriptWindow();
        if (IsLevelUp)
        {
            return;
        }
        currentExp += expPerTouch;
        if (currentExp >= maxExp)
        {
            IsLevelUp = true;
            LevelUp();
        }
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
        IsLevelUp = false;
    }

    
}
