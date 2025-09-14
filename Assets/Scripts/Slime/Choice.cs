using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    private void OnYesButtonClicked()
    {
        // Yes: 현재 생성된 슬라임을 그대로 유지하고 키우기
        Debug.Log("Yes 버튼 클릭: 현재 슬라임을 계속 키웁니다.");
        
        // Choice UI 비활성화
        gameObject.SetActive(false);
        
        // 현재 슬라임이 있다면 그대로 유지 (아무것도 하지 않음)
        // 슬라임이 계속 성장할 수 있도록 상태 유지
    }

    private void OnNoButtonClicked()
    {
        // No: 현재 슬라임을 파괴하고 기본 슬라임(Normal) 생성
        Debug.Log("No 버튼 클릭: 기본 슬라임을 새로 생성합니다.");
        
        // SlimeManager 찾기
        GameObject slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        if (slimeManagerObject != null)
        {
            SlimeManager slimeManager = slimeManagerObject.GetComponent<SlimeManager>();
            
            // 현재 슬라임이 있다면 파괴
            if (slimeManager.HasCurrentSlime())
            {
                slimeManager.DestroySlime();
            }
            
            // 기본 슬라임(Normal) 생성
            slimeManager.CreateSlime(SlimeType.Normal);
        }
        else
        {
            Debug.LogError("SlimeManager를 찾을 수 없습니다!");
        }
        
        // Choice UI 비활성화
        gameObject.SetActive(false);
    }


}
