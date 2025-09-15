using UnityEngine;
using UnityEngine.UI;

public class MaxLevelPanel : MonoBehaviour
{
    [SerializeField] private Button maxLevelPanelCloseButton;
    [SerializeField] public SlimeManager slimeManager;
    [SerializeField] private GameObject levelText;
    [SerializeField] private GameObject levelBar;
    [SerializeField] private UiManager uiManager;
    public bool IsClosed { get; private set; } = false;

    private void Start()
    {
        maxLevelPanelCloseButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
        levelText.SetActive(false);
        levelBar.SetActive(false);
        //uiManager.IsMaxPanelActive = false;
        //slimeManager.SlimeDestroyed = true;
    }
}
