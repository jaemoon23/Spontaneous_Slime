using System;
using UnityEngine;

public class InteriorManager : MonoBehaviour
{
    public static event Action OnWindowActivated;
    public static event Action OnWoolenYarnActivated;
    public static InteriorManager Instance { get; private set; }    // 싱글톤 인스턴스
    [SerializeField] private GameObject aircon; // 에어컨
    [SerializeField] private GameObject stove;  // 난로
    [SerializeField] private GameObject interiorLight; // 인테리어 조명
    [SerializeField] private GameObject flowerPot; // 화분
    [SerializeField] private GameObject humidifier; // 가습기
    [SerializeField] private GameObject window; // 창문
    [SerializeField] private GameObject clock; // 시계
    [SerializeField] private GameObject woolenYarn; // 양털

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
    }
    public bool GetAirconActive()
    {
        return aircon != null && aircon.activeSelf;
    }
    public bool GetStoveActive()
    {
        return stove != null && stove.activeSelf;
    }
    public bool GetInteriorLightActive()
    {
        return interiorLight != null && interiorLight.activeSelf;
    }
    public bool GetFlowerPotActive()
    {
        return flowerPot != null && flowerPot.activeSelf;
    }
    public bool GetHumidifierActive()
    {
        return humidifier != null && humidifier.activeSelf;
    }
    public bool GetWindowActive()
    {
        return window != null && window.activeSelf;
    }
    public bool GetClockActive()
    {
        return clock != null && clock.activeSelf;
    }
    public bool GetWoolenYarnActive()
    {
        return woolenYarn != null && woolenYarn.activeSelf;
    }

    public void SetWoolenYarnActive(bool isWoolenYarn)
    {
        if (woolenYarn != null)
        {
            woolenYarn.SetActive(isWoolenYarn);
            if (isWoolenYarn)
            {
                OnWoolenYarnActivated.Invoke();
            }
        }
    }

    // 인테리어 상태 저장
    public void SaveInteriorStates()
    {
        var saveData = SaveLoadManager.Data;
        
        saveData.IsAirConditionerActive = GetAirconActive();
        saveData.IsStoveActive = GetStoveActive();
        saveData.IsLightActive = GetInteriorLightActive();
        saveData.IsFlowerPotActive = GetFlowerPotActive();
        saveData.IsHumidifierActive = GetHumidifierActive();
        saveData.IsWindowActive = GetWindowActive();
        saveData.IsClockActive = GetClockActive();
        saveData.IsWoolenYarnActive = GetWoolenYarnActive();
        
        Debug.Log("인테리어 상태 저장 완료");
    }
    
    // 인테리어 상태 로드
    public void LoadInteriorStates()
    {
        var saveData = SaveLoadManager.Data;
        
        if (saveData == null)
        {
            Debug.LogError("[인테리어 로드] SaveLoadManager.Data가 null입니다!");
            return;
        }
        
        Debug.Log($"[인테리어 로드] 시작 - 에어컨:{saveData.IsAirConditionerActive}, 스토브:{saveData.IsStoveActive}, 조명:{saveData.IsLightActive}, 화분:{saveData.IsFlowerPotActive}, 가습기:{saveData.IsHumidifierActive}, 창문:{saveData.IsWindowActive}, 시계:{saveData.IsClockActive}, 양털:{saveData.IsWoolenYarnActive}");
        
        if (aircon != null) 
        {
            aircon.SetActive(saveData.IsAirConditionerActive);
            Debug.Log($"[인테리어 로드] 에어컨 설정: {saveData.IsAirConditionerActive}");
        }
        if (stove != null) 
        {
            stove.SetActive(saveData.IsStoveActive);
            Debug.Log($"[인테리어 로드] 스토브 설정: {saveData.IsStoveActive}");
        }
        if (interiorLight != null) 
        {
            interiorLight.SetActive(saveData.IsLightActive);
            Debug.Log($"[인테리어 로드] 인테리어 조명 설정: {saveData.IsLightActive}");
        }
        if (flowerPot != null) 
        {
            flowerPot.SetActive(saveData.IsFlowerPotActive);
            Debug.Log($"[인테리어 로드] 화분 설정: {saveData.IsFlowerPotActive}");
        }
        if (humidifier != null) 
        {
            humidifier.SetActive(saveData.IsHumidifierActive);
            Debug.Log($"[인테리어 로드] 가습기 설정: {saveData.IsHumidifierActive}");
        }
        if (window != null) 
        {
            window.SetActive(saveData.IsWindowActive);
            Debug.Log($"[인테리어 로드] 창문 설정: {saveData.IsWindowActive}");
        }
        if (clock != null) 
        {
            clock.SetActive(saveData.IsClockActive);
            Debug.Log($"[인테리어 로드] 시계 설정: {saveData.IsClockActive}");
        }
        if (woolenYarn != null) 
        {
            woolenYarn.SetActive(saveData.IsWoolenYarnActive);
            Debug.Log($"[인테리어 로드] 양털 설정: {saveData.IsWoolenYarnActive}");
        }
        
        // 이벤트 발생 (활성화된 경우)
        if (saveData.IsWindowActive && window != null)
        {
            OnWindowActivated?.Invoke();
        }
        if (saveData.IsWoolenYarnActive && woolenYarn != null)
        {
            OnWoolenYarnActivated?.Invoke();
        }
        
        Debug.Log("인테리어 상태 로드 완료");
    }

}
