using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button menuCloseButton;
    [SerializeField] private Button menuOpenButton;

    private void Start()
    {
        menuCloseButton.onClick.AddListener(CloseMenu);
        menuOpenButton.onClick.AddListener(OpenMenu);
    }

    private void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
    private void OpenMenu()
    {
        menuPanel.SetActive(true);
    }
}
