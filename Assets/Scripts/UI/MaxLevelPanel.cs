using UnityEngine;
using UnityEngine.UI;
using Excellcube.EasyTutorial.Utils;

public class MaxLevelPanel : MonoBehaviour
{
    [SerializeField] private Button maxLevelPanelCloseButton;
    [SerializeField] public SlimeManager slimeManager;
    [SerializeField] private GameObject levelText;
    [SerializeField] private GameObject levelBar;
    private GameObject slimeObject; // 슬라임 매니저 오브젝트 참조
    private SlimeGrowth slimeGrowth;
    [SerializeField] private UiManager uiManager;
    public bool IsClosed { get; private set; } = false;
    [SerializeField] private Button collectionButton;

    private void Start()
    {
        slimeObject = GameObject.FindWithTag(Tags.Player);
        slimeGrowth = slimeObject.GetComponent<SlimeGrowth>();

        maxLevelPanelCloseButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false); // 패널 비활성화

        levelText.SetActive(false); // 레벨 텍스트 비활성화
        levelBar.SetActive(false);  // 레벨 바 비활성화
        collectionButton.interactable = true; // 도감 버튼 활성화
        slimeGrowth.SetMaxLevelState(true); // SlimeGrowth에 최대 레벨 상태 설정 알림
        if (PlayerPrefs.GetInt("ECET_CLEAR_ALL") == 0)
        {
            TutorialEvent.Instance.Broadcast("MAX_LEVEL_PANEL_CLOSED");
        }

    }
}
