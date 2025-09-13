using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlimeManager slime; // 슬라임 참조
    private GameObject slimeManager; // 슬라임 오브젝트 참조
    public bool isFirstStart { get; set; } = true;

    private void Awake()
    {
        // 게임 시작 시 최초 실행 여부를 확인
        if (isFirstStart)
        {
            isFirstStart = true;
            Debug.Log("게임이 처음 시작되었습니다.");
        }
    }
    private void Start()
    {
        
        slimeManager = GameObject.FindWithTag(Tags.SlimeManager);
        slime = slimeManager.GetComponent<SlimeManager>();
    }

    public SlimeType GetSlimeTypeByEnvironment()
    {
        // TODO: 환경 조건에 맞춰서 슬라임 타입 결정

        return SlimeType.Normal;
    }
}
