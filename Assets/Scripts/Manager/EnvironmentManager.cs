using TMPro;
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
