using System.Collections;
using UnityEngine;

public class SlimeGrowth : MonoBehaviour, ITouchable
{
    public bool IsLevelUp { get; private set; } = false;

    // 불필요한 리스트 제거
    // private List<LevelUpData> levelUpDataList = new List<LevelUpData>();
    
    private int index = 0;
    [SerializeField] private int expPerTouch = 1;
    private int currentExp = 0;
    public int Level { get; private set; }
    private int maxExp;
    private int scaleLevel;
    private Vector3 baseScale;
    private Reward reward;

    private void Start()
    {
        baseScale = transform.localScale;
        reward = GetComponent<Reward>();

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
            Debug.Log("최고 레벨 도달");
            return;
        }

        index++;
        // 새 레벨 데이터 직접 접근
        var levelData = DataTableManager.LevelUpTable.Get(DataTableIds.LevelUpIds[index]);
        if (levelData != null)
        {
            Level = levelData.CurrentLevel;
            currentExp -= maxExp;
            maxExp = levelData.NeedExp;
            scaleLevel = levelData.ScaleLevel;
            StartCoroutine(CoScaleUpDown(1f));

            // 10레벨 달성 시 리워드 지급
            if (Level == 10 && reward != null)
            {
                reward.GiveReward();
            }
        }
    }

    public void OnTouch()
    {
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
