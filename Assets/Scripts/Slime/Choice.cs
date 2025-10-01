using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject levelText;
    [SerializeField] private GameObject windowPanel;
    private SlimeType type;

    private void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }
    private void OnEnable()
    {
        windowPanel.SetActive(false);
    }
    private void OnYesButtonClicked()
    {
        // Yes: 현재 생성된 슬라임을 그대로 유지하고 키우기
        Debug.Log("Yes 버튼 클릭: 현재 슬라임을 계속 키웁니다.");

        // Choice UI 비활성화
        gameObject.SetActive(false);
        slider.SetActive(true);
        levelText.SetActive(true);
    }

    private void OnNoButtonClicked()
    {
        slider.SetActive(false);
        levelText.SetActive(false);
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
