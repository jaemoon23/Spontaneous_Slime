using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeGrowth : MonoBehaviour, ITouchable
{
    public bool IsLevelUp { get; private set; } = false;

    private List<LevelUpData> levelUpDataList = new List<LevelUpData>(); // 레벨업 데이터 리스트
    private int index = 0; // 현재 레벨 인덱스
    [SerializeField] private int expPerTouch = 1; // 터치당 경험치
    private int currentExp = 0; // 현재 경험치
    public int Level { get; private set; } // 현재 레벨
    [SerializeField] private int maxLevel = 10; // 최대 레벨
    private int maxExp; // 레벨업에 필요한 경험치
    private int scaleLevel; // 레벨업 시 크기 배율
    private Vector3 baseScale;
    private Reward reward;
    private void Start()
    {
        baseScale = transform.localScale;
        reward = GetComponent<Reward>();

        foreach (var id in DataTableIds.LevelUpIds)
        {
            var levelData = DataTableManager.LevelUpTable.Get(id);
            levelUpDataList.Add(levelData);
        }
        Level = levelUpDataList[index].CurrentLevel;
        maxExp = levelUpDataList[index].NeedExp;
        scaleLevel = levelUpDataList[index].ScaleLevel;
    }
    private void Update()
    {
        // TODO: 최종 빌드전에 삭제 필요
        // 테스트용: 1키를 누르면 강제로 레벨업 로직 실행
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!IsLevelUp)
            {
                IsLevelUp = true;
                LevelUp();
                // TODO: 최종 빌드전에 삭제 필요
                Debug.Log($"테스트: 강제 레벨업 실행\n현재 레벨: {Level}");
            }
        }
    }
    private void LevelUp()
    {
        if (index >= levelUpDataList.Count - 1)
        {
            // TODO: 최종 빌드전에 삭제 필요
            Debug.Log("최고 레벨 도달");
            return;
        }

        index++;
        Level = levelUpDataList[index].CurrentLevel;
        currentExp -= maxExp;
        maxExp = levelUpDataList[index].NeedExp;
        scaleLevel = levelUpDataList[index].ScaleLevel;
        StartCoroutine(CoScaleUpDown(1f));

        if (Level == maxLevel && reward != null)
        {
            reward.GiveReward();
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
        // TODO: 최종 빌드전에 삭제 필요
        Debug.Log($"경험치 :{currentExp} / {maxExp}");
        Debug.Log($"레벨 :{Level}");
    }

    private IEnumerator CoScaleUpDown(float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = baseScale * scaleLevel; // 목표 크기 계산
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        transform.localScale = endScale; // 마지막 보정
        IsLevelUp = false;
    }

}
