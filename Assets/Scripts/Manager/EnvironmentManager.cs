using UnityEngine;
using System;


public class EnvironmentManager : MonoBehaviour
{
    // 환경 변화 이벤트
    public static event Action OnEnvironmentChanged;
    private int airconTemp;
    private int stoveStep;
    private int lightStep;
    private bool isFlower = false;
    private int humidity;

    private void Start()
    {

        var airconItem = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.AirConditioner]);
        var tempItem = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.Heater]);
        var lightItem = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.Light]);
        var humidItem = DataTableManager.ItemTable.Get(DataTableIds.ItemIds[(int)EnvironmentType.Humidifier]);

        // 초기값 설정
        airconTemp = airconItem != null ? airconItem.DefaultValue : 10; // null 일경우 명시적으로 10 할당
        stoveStep = tempItem != null ? tempItem.DefaultValue : 1; // null 일경우 명시적으로 1 할당
        lightStep = lightItem != null ? lightItem.DefaultValue : 0; // null 일경우 명시적으로 0 할당
        humidity = humidItem != null ? humidItem.DefaultValue : 0; // null 일경우 명시적으로 0 할당
        isFlower = false;

        Debug.Log($"초기 환경값 설정 완료 - 에어컨: {airconTemp}, 난로: {stoveStep}, 조명: {lightStep}, 습도: {humidity}");
    }

    public int AirconTemp
    {
        get => airconTemp;
        set
        {
            if (airconTemp != value)
            {
                airconTemp = value;
                OnEnvironmentChanged?.Invoke();
            }
        }
    }

    public int StoveStep
    {
        get => stoveStep;
        set
        {
            if (stoveStep != value)
            {
                stoveStep = value;
                OnEnvironmentChanged?.Invoke();
            }
        }
    }

    public int LightStep
    {
        get => lightStep;
        set
        {
            if (lightStep != value)
            {
                lightStep = value;
                OnEnvironmentChanged?.Invoke();
            }
        }
    }

    public bool IsFlower
    {
        get => isFlower;
        set
        {
            if (isFlower != value)
            {
                isFlower = value;
                OnEnvironmentChanged?.Invoke();
            }
        }
    }

    public int Humidity
    {
        get => humidity;
        set
        {
            if (humidity != value)
            {
                humidity = value;
                OnEnvironmentChanged?.Invoke();
            }
        }
    }

    public GameObject[] panels; // 모든 패널을 배열로 관리
    public void ActivatePanel(GameObject targetPanel)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(panel == targetPanel); // targetPanel만 활성화, 나머지는 비활성화
        }
    }
}
