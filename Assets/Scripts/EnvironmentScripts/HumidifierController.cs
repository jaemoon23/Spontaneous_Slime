using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumidifierController : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject dehumidifierWindow;
    [SerializeField] private Slider slider;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private TextMeshProUGUI humidifierText;
    private int minHumidity = 0;
    private int maxHumidity = 100;
    private string textFormat = "현재 습도는 {000}%입니다.";
        private string text = "{000}";
    private void Start()
    {
        var item = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.Humidifier]);
        var textData = DataTableManager.StringTable.Get(item.UIText);
        textFormat = textData != null ? textData.Value : this.textFormat;

        if (slider == null && dehumidifierWindow != null)
        {
            slider = dehumidifierWindow.GetComponentInChildren<Slider>(true);
        }
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.minValue = minHumidity;
        slider.maxValue = maxHumidity;
        slider.value = environmentManager.Humidity;
    }
    private void OnEnable()
    {
        environmentManager.Humidity = 0;
        slider.value = environmentManager.Humidity;
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnSliderChanged);
        }
        humidifierText.text = textFormat.Replace(text, environmentManager.Humidity.ToString());
    }
    public void OnTouch()
    {
        // TODO: Debug log 제거
        Debug.Log("제습기 터치됨");
        windowPanel.SetActive(true);
        environmentManager.ActivatePanel(dehumidifierWindow);
    }
    public void OnSliderChanged(float value)
    {
        int rounded = Mathf.RoundToInt(value / 10) * 10;
        slider.value = rounded;
        environmentManager.Humidity = rounded;
        Debug.Log($"슬라이더 값 변경됨: {environmentManager.Humidity}");
        humidifierText.text = textFormat.Replace(text, environmentManager.Humidity.ToString());
    }
}
