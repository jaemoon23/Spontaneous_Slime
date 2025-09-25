using UnityEngine;

public class HintManager : MonoBehaviour
{
    // 현재 슬라임이 있는지 확인

    // 없다면 환경 이벤트 확인 후 힌트 제공 (환경 이벤트는 각 환경이 변하는 이벤트를 만들고 그 이벤트가 발생했을 때 값을 체크해서 힌트제공)

    [Header("References")]
    [SerializeField] private SlimeManager slimeManager;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private TimeManager timeManager;

    private bool IsCat = false;
    private void Start()
    {
        EnvironmentManager.OnAirconChanged += CheckAirconChange;
        EnvironmentManager.OnStoveChanged += CheckStoveChange;
        EnvironmentManager.OnLightChanged += CheckLightChange;
        EnvironmentManager.OnHumidityChanged += CheckHumidityChange;

        TimeManager.OnTimeChanged += CheckTime;
        TimeManager.OnWeatherChanged += CheckWindow;
        InteriorManager.OnWindowActivated += CheckWindow;
        InteriorManager.OnWoolenYarnActivated += CheckWoolenYarn;
    }
    private void OnDestroy()
    {
        EnvironmentManager.OnAirconChanged -= CheckAirconChange;
        EnvironmentManager.OnStoveChanged -= CheckStoveChange;
        EnvironmentManager.OnLightChanged -= CheckLightChange;
        EnvironmentManager.OnHumidityChanged -= CheckHumidityChange;

        TimeManager.OnTimeChanged -= CheckTime;
        TimeManager.OnWeatherChanged -= CheckWindow;
        InteriorManager.OnWindowActivated -= CheckWindow;
        InteriorManager.OnWoolenYarnActivated -= CheckWoolenYarn;
    }

    // 에어컨 변화시 힌트를 줘야 하는지 체크
    private void CheckAirconChange()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (environmentManager.AirconTemp == 5)
        {
            // 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_122030");
        }
    }

    // 난로 변화시 힌트를 줘야 하는지 체크
    private void CheckStoveChange()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (environmentManager.StoveStep == 4)
        {
            // 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_141040");
        }
    }

    // 조명 변화시 힌트를 줘야 하는지 체크
    private void CheckLightChange()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (environmentManager.LightStep == 3 && InteriorManager.Instance.GetFlowerPotActive())
        {
            // 식물 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_203010");
        }
        if (environmentManager.LightStep == 1)
        {
            // 어둠 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_112010");
        }
        if (environmentManager.LightStep == 4)
        {
            // 빛 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_111010");
        }
    }

    // 습도 변화시 힌트를 줘야 하는지 체크
    private void CheckHumidityChange()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (environmentManager.Humidity == 40 && InteriorManager.Instance.GetFlowerPotActive())
        {
            // 식물 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_203020");
        }
        if (environmentManager.Humidity == 90)
        {
            // 물 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_121020");
        }
    }


    // 창문이 있을때
    private void CheckWindow()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (InteriorManager.Instance.GetWindowActive() && timeManager.CurrentWeather == TimeManager.WeatherState.Clear)
        {
            // 비 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_221120");
        }

        if (InteriorManager.Instance.GetWindowActive() && timeManager.CurrentWeather == TimeManager.WeatherState.Rain)
        {
            // 오로라 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_231130");
        }

    }
    // 시간이 맞을때 힌트를 줘야 하는지 체크
    private void CheckTime()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (timeManager.CurrentTimeOfDay == TimeManager.TimeState.Night && InteriorManager.Instance.GetClockActive())
        {
            // 오로라 슬라임 힌트 제공
            uiManager.ShowHintText("HINT_SCRIPT_231140");
        }

    }
    private void CheckWoolenYarn()
    {
        if (slimeManager.HasCurrentSlime())
        {
            return;
        }
        if (InteriorManager.Instance.GetWoolenYarnActive())
        {
            // 고양이 슬라임 힌트 제공
            if (IsCat)
            {
                return;
            }
            uiManager.ShowHintText("HINT_SCRIPT_251150");
            IsCat = true;
        }
    }

    
}
