using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    [SerializeField] private Button CloseButton;
    [SerializeField] private TextMeshProUGUI warningText;

    private void Start()
    {
        CloseButton.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowWarning(string message)
    {
        gameObject.SetActive(true);
        warningText.text = message;
    }
}
