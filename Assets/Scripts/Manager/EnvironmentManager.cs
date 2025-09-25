using UnityEngine;
using System;


public class EnvironmentManager : MonoBehaviour
{
    // 환경 변화 이벤트
    public static event Action OnEnvironmentChanged;
    public static event Action OnAirconChanged;
    public static event Action OnStoveChanged;
    public static event Action OnLightChanged;
    public static event Action OnHumidityChanged;
    private int airconTemp;
    private int stoveStep;
    private int lightStep;
    private bool isFlower = false;
    private int humidity;

    private void Start()
    {

        var airconItem = DataTableManager.InteriorTable.Get(DataTableIds.InteriorIds[(int)EnvironmentType.AirConditioner]);
        var tempItem = DataTableManager.InteriorTable.Get(DataTableIds.InteriorIds[(int)EnvironmentType.Heater]);
        var lightItem = DataTableManager.InteriorTable.Get(DataTableIds.InteriorIds[(int)EnvironmentType.Light]);
        var humidItem = DataTableManager.InteriorTable.Get(DataTableIds.InteriorIds[(int)EnvironmentType.Humidifier]);

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
                OnAirconChanged?.Invoke();
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
                OnStoveChanged?.Invoke();
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
                OnLightChanged?.Invoke();
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
                OnHumidityChanged?.Invoke();
            }
        }
    }

    public GameObject[] panels; // 모든 패널을 배열로 관리
    
    [Header("Environment Objects")]
    [SerializeField] private GameObject airConditionerObject;
    [SerializeField] private GameObject humidifierObject;
    [SerializeField] private GameObject lightObject;
    [SerializeField] private GameObject stoveObject;
    [SerializeField] private GameObject flowerPotObject;
    
    public void ActivatePanel(GameObject targetPanel)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(panel == targetPanel); // targetPanel만 활성화, 나머지는 비활성화
        }
    }
    
    // 환경 오브젝트 활성 상태를 SaveData에 저장
    public void SaveEnvironmentObjectStates()
    {
        var saveData = SaveLoadManager.Data;
        
        saveData.IsAirConditionerActive = airConditionerObject != null && airConditionerObject.activeSelf;
        saveData.IsHumidifierActive = humidifierObject != null && humidifierObject.activeSelf;
        saveData.IsLightActive = lightObject != null && lightObject.activeSelf;
        saveData.IsStoveActive = stoveObject != null && stoveObject.activeSelf;
        saveData.IsFlowerPotActive = flowerPotObject != null && flowerPotObject.activeSelf;
        
        Debug.Log($"환경 오브젝트 상태 저장: 에어컨={saveData.IsAirConditionerActive}, 제습기={saveData.IsHumidifierActive}, 조명={saveData.IsLightActive}, 난로={saveData.IsStoveActive}, 화분={saveData.IsFlowerPotActive}");
    }
    
    // SaveData에서 환경 오브젝트 활성 상태 로드
    public void LoadEnvironmentObjectStates()
    {
        var saveData = SaveLoadManager.Data;

        if (airConditionerObject != null)
        {
            airConditionerObject.SetActive(saveData.IsAirConditionerActive);
        }
        if (humidifierObject != null)
        {
            humidifierObject.SetActive(saveData.IsHumidifierActive);
        }
        if (lightObject != null)
        {
            lightObject.SetActive(saveData.IsLightActive);
        }
        if (stoveObject != null)
        {
            stoveObject.SetActive(saveData.IsStoveActive);
        }
        if (flowerPotObject != null)
        {
            flowerPotObject.SetActive(saveData.IsFlowerPotActive);
        }
            
        Debug.Log($"환경 오브젝트 상태 로드: 에어컨={saveData.IsAirConditionerActive}, 제습기={saveData.IsHumidifierActive}, 조명={saveData.IsLightActive}, 난로={saveData.IsStoveActive}, 화분={saveData.IsFlowerPotActive}");
    }
}
