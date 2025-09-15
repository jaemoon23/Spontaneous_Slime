using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    private SlimeType type;

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
        // No: 현재 슬라임을 내보내기
        // TODO: 내보낸 슬라임의 타입을 저장해두고 생성이 안되게 막음 (1회만)
        // TODO: 현재 슬라임: 어둠 슬라임 > 내보내기 > 어둠 슬라임 생성 막음 > 현재 슬라임 불 슬라임 
        // TODO: > 내보내기 > 어둠 슬라임 제한 해제, 불 슬라임 생성 제한 > 키우기 선택 까지 반복


        Debug.Log("No 버튼 클릭: 현재 슬라임을 내보냅니다.");
        
        // SlimeManager 찾기
        GameObject slimeManagerObject = GameObject.FindWithTag(Tags.SlimeManager);
        if (slimeManagerObject != null)
        {
            SlimeManager slimeManager = slimeManagerObject.GetComponent<SlimeManager>();
            
            // 현재 슬라임이 있다면 자유롭게 해주기
            if (slimeManager.HasCurrentSlime())
            {
                SlimeType type = slimeManager.GetCurrentSlimeType();
                slimeManager.SlimeFree();
            }
        }
        else
        {
            Debug.LogError("SlimeManager를 찾을 수 없습니다!");
        }
        
        // Choice UI 비활성화
        gameObject.SetActive(false);
    }


}
