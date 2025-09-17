using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirConditioner : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject airConditionerWindow;
    [SerializeField] private Slider slider;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private TextMeshProUGUI airconText;
    private int minTemp = 0;
    private int maxTemp = 20;
    private string textFormat = "현재 온도는 {000}°C입니다.";
    private string text = "{000}";
    private void Start()
    {
        var item = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.AirConditioner]);
        var textData = DataTableManager.StringTable.Get(item.UIText);

        textFormat = textData != null ? textData.Value : this.textFormat;

        if (slider == null && airConditionerWindow != null)
        {
            slider = airConditionerWindow.GetComponentInChildren<Slider>(true);
        }
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.minValue = minTemp;
        slider.maxValue = maxTemp;

    }

    private void OnEnable()
    {
        slider.value = environmentManager.AirconTemp;
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnSliderChanged);
        }
        airconText.text = textFormat.Replace(text, environmentManager.AirconTemp.ToString());
    }

    public void OnTouch()
    {
        // TODO: Debug log 제거
        Debug.Log("에어컨 터치됨");
        windowPanel.SetActive(true);
        environmentManager.ActivatePanel(airConditionerWindow);
    }

    public void OnSliderChanged(float value)
    {
        int rounded = Mathf.RoundToInt(value / 5) * 5;
        slider.value = rounded;
        environmentManager.AirconTemp = rounded;
        Debug.Log($"슬라이더 값 변경됨: {environmentManager.AirconTemp}");
        airconText.text = textFormat.Replace(text, environmentManager.AirconTemp.ToString());
    }
}
