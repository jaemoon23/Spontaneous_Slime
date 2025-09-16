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
    private string textFormat = "조명: {0}단계";
    private string text = "{0}";
    private void Start()
    {
        var item = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.Light]);
        var textData = DataTableManager.StringTable.Get(item.UIText);
        textFormat = textData != null ? textData.Value : this.textFormat;


        plusButton.onClick.AddListener(OnClickPlusLight);
        minusButton.onClick.AddListener(OnClickMinusLight);
        lightText.text = textFormat.Replace(text, environmentManager.LightStep.ToString());
        textField.text = $"{environmentManager.LightStep}";
    }
    private void OnEnable()
    {
        environmentManager.LightStep = 10;
    }
    public void OnTouch()
    {
        Debug.Log("조명 터치됨");
        windowPanel.SetActive(true);
        environmentManager.ActivatePanel(lightWindow);
    }

    public void OnClickPlusLight()
    {
        if (environmentManager.LightStep < maxLightStep)
        {
            environmentManager.LightStep += 1;
            textField.text = $"{environmentManager.LightStep}";
            lightText.text = textFormat.Replace(text, environmentManager.LightStep.ToString());
            
        }
    }

    public void OnClickMinusLight()
    {
        if (environmentManager.LightStep > minLightStep)
        {
            environmentManager.LightStep -= 1;
            textField.text = $"{environmentManager.LightStep}";
            lightText.text = textFormat.Replace(text, environmentManager.LightStep.ToString());
        }
    }
}
