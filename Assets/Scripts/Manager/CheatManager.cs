using UnityEngine;

public class CheatManager : MonoBehaviour
{
    [Header("환경 설정")]
    public GameObject[] Environments;
    public SlimeManager slimeManager;
    
    [Header("치트 버튼")]
    [Space(10)]
    public bool activateEnvironments;
    public bool deactivateEnvironments;
    public bool maxLevelSlimeCheat;
    
    private void Update()
    {
        // 환경 오브젝트 활성화
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (var env in Environments)
            {
                env.SetActive(true);
            }
        }

        // 환경 오브젝트 비활성화
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var env in Environments)
            {
                env.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 슬라임을 최대 레벨까지 성장시키고 완료 처리
            MaxLevelSlimeCheat();
        }

        // 인스펙터 치트 버튼 처리
        HandleInspectorCheats();
    }

    private void HandleInspectorCheats()
    {
        // 환경 활성화 치트
        if (activateEnvironments)
        {
            activateEnvironments = false;
            foreach (var env in Environments)
            {
                env.SetActive(true);
            }
            Debug.Log("치트: 모든 환경 오브젝트 활성화됨");
        }

        // 환경 비활성화 치트
        if (deactivateEnvironments)
        {
            deactivateEnvironments = false;
            foreach (var env in Environments)
            {
                env.SetActive(false);
            }
            Debug.Log("치트: 모든 환경 오브젝트 비활성화됨");
        }

        // 슬라임 최대 레벨 치트
        if (maxLevelSlimeCheat)
        {
            maxLevelSlimeCheat = false;
            MaxLevelSlimeCheat();
        }
    }

    private void MaxLevelSlimeCheat()
    {
        if (slimeManager == null || !slimeManager.HasCurrentSlime())
        {
            Debug.LogWarning("치트: 현재 슬라임이 없습니다!");
            return;
        }

        // 매번 현재 슬라임에서 SlimeGrowth를 찾기
        SlimeGrowth currentSlimeGrowth = slimeManager.GetCurrentSlime().GetComponent<SlimeGrowth>();
        if (currentSlimeGrowth == null)
        {
            Debug.LogError("치트: 현재 슬라임에서 SlimeGrowth 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        Debug.Log("치트: 슬라임을 최대 레벨까지 성장시킵니다!");

        // 현재 슬라임의 최대 레벨까지 경험치를 채우면서 레벨업
        int maxLevel = currentSlimeGrowth.MaxLevel;
        int currentLevel = currentSlimeGrowth.Level;
        
        Debug.Log($"치트: 현재 레벨 {currentLevel} → 최대 레벨 {maxLevel}로 성장 시작");

        // 최대 레벨까지 for문으로 경험치 채우기
        for (int targetLevel = currentLevel; targetLevel < maxLevel; targetLevel++)
        {
            // 현재 레벨에서 필요한 경험치만큼 채우기
            int currentExp = currentSlimeGrowth.CurrentExp;
            int maxExp = currentSlimeGrowth.MaxExp;
            int neededExp = maxExp - currentExp;

            Debug.Log($"치트: 레벨 {targetLevel} → {targetLevel + 1}, 필요 경험치: {neededExp}");

            // 필요한 만큼 경험치 추가 (OnTouch 시뮬레이션)
            for (int i = 0; i < neededExp; i++)
            {
                currentSlimeGrowth.AddExpCheat();
                
                // 레벨업이 일어났으면 다음 레벨로
                if (currentSlimeGrowth.Level > targetLevel)
                {
                    break;
                }
            }
        }

        // 최대 레벨에 도달했다면 최종 경험치를 최대치로 채우기
        if (currentSlimeGrowth.Level >= maxLevel)
        {
            int finalNeededExp = currentSlimeGrowth.MaxExp - currentSlimeGrowth.CurrentExp;
            Debug.Log($"치트: 최대 레벨 도달! 최종 경험치 {finalNeededExp} 추가");
            
            for (int i = 0; i < finalNeededExp; i++)
            {
                currentSlimeGrowth.AddExpCheat();
            }
        }

        Debug.Log($"치트: 슬라임 성장 완료! 최종 레벨: {currentSlimeGrowth.Level}, 경험치: {currentSlimeGrowth.CurrentExp}/{currentSlimeGrowth.MaxExp}");
    }
    
    [ContextMenu("환경 오브젝트 모두 활성화")]
    private void ActivateAllEnvironments()
    {
        foreach (var env in Environments)
        {
            env.SetActive(true);
        }
        Debug.Log("치트: 모든 환경 오브젝트 활성화됨");
    }

    [ContextMenu("환경 오브젝트 모두 비활성화")]
    private void DeactivateAllEnvironments()
    {
        foreach (var env in Environments)
        {
            env.SetActive(false);
        }
        Debug.Log("치트: 모든 환경 오브젝트 비활성화됨");
    }

    [ContextMenu("슬라임 최대 레벨로 성장")]
    private void MaxLevelSlimeCheatMenu()
    {
        MaxLevelSlimeCheat();
    }
}