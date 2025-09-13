using TMPro;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public int AirconTemp { get; set; } = -10;// 에어컨 온도로 표시
    public int StoveStep { get; set; } = 10;// 난로 단계로 표시
    public int LightStep { get; set; } = 0;// 조명 단계로 표시 
    public bool IsFlower { get; set; } = false;// 화분 단계로 표시
    public int Humidity { get; set; } = 10; // 습도 %로 표시

    [SerializeField] private TextMeshProUGUI humidityText;
    
    [SerializeField] private TextMeshProUGUI stoveText;


    private void Update()
    {
        UpdateEnvironmentStatusUI();
    }

    public void UpdateEnvironmentStatusUI()
    {

        humidityText.text = $"습도: {Humidity}%";
        stoveText.text = $"난로: {StoveStep}단계";
    }
    
    
}
