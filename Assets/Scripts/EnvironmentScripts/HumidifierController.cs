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
    [SerializeField] private int minHumidity = 0;
    [SerializeField] private int maxHumidity = 100;
    private void Start()
    {
        if (slider == null && dehumidifierWindow != null)
        {
            slider = dehumidifierWindow.GetComponentInChildren<Slider>(true);
        }
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.minValue = minHumidity;
        slider.maxValue = maxHumidity;
        slider.value = environmentManager.AirconTemp;
    }
    public void OnTouch()
    {
        // TODO: Debug log 제거
        Debug.Log("제습기 터치됨");
        windowPanel.SetActive(true);
        dehumidifierWindow.SetActive(true);
    }
    public void OnSliderChanged(float value)
    {
        int rounded = Mathf.RoundToInt(value / 5) * 5;
        slider.value = rounded;
        environmentManager.Humidity = rounded;
        Debug.Log($"슬라이더 값 변경됨: {environmentManager.Humidity}");
        humidifierText.text = $"습도: {environmentManager.Humidity}%";
    }
}
