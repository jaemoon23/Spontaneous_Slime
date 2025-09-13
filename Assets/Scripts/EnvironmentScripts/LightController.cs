using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour, ITouchable
{
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject lightWindow;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private TextMeshProUGUI lightText;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private int minLightStep = 0;
    [SerializeField] private int maxLightStep = 20;

    private void Start()
    {
        plusButton.onClick.AddListener(OnClickPlusLight);
        minusButton.onClick.AddListener(OnClickMinusLight);
        lightText.text = $"조명: {environmentManager.LightStep}단계";
        textField.text = $"{environmentManager.LightStep}";
    }
    public void OnTouch()
    {
        Debug.Log("조명 터치됨");
        windowPanel.SetActive(true);
        lightWindow.SetActive(true);
    }

    public void OnClickPlusLight()
    {
        if (environmentManager.LightStep < maxLightStep)
        {
            environmentManager.LightStep += 1;
            lightText.text = $"조명: {environmentManager.LightStep}단계";
            textField.text = $"{environmentManager.LightStep}";
            
        }
    }

    public void OnClickMinusLight()
    {
        if (environmentManager.LightStep > minLightStep)
        {
            environmentManager.LightStep -= 1;
            lightText.text = $"조명: {environmentManager.LightStep}단계";
            textField.text = $"{environmentManager.LightStep}";
        }
    }
}
