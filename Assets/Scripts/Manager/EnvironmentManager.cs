using TMPro;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // TODO: 환경 초기 세팅값 변경하실거면 여기서 수치만 바꾸시면 됩니다.
    public int AirconTemp { get; set; } = -10;// 에어컨 온도로 표시
    public int StoveStep { get; set; } = 0;// 난로 단계로 표시
    public int LightStep { get; set; } = 0;// 조명 단계로 표시
    public bool IsFlower { get; set; } = false;// 화분 활성 여부
    public int Humidity { get; set; } = 0; // 습도 %로 표시

    public GameObject[] panels; // 모든 패널을 배열로 관리
    public void ActivatePanel(GameObject targetPanel)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(panel == targetPanel); // targetPanel만 활성화, 나머지는 비활성화
        }
    }
}
