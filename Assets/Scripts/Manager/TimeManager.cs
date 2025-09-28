using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeState
    {
        Morning, // 아침
        Afternoon, // 오후
        Evening, // 저녁
        Night // 새벽/밤
    }
    public enum WeatherState
    {
        Clear, // 맑음
        Rain,  // 비
    }
    public static event Action<int> OnDayPassed;

    public static event System.Action OnTimeChanged;
    public static event System.Action OnWeatherChanged;

    [SerializeField] public GameManager gameManager;
    public WeatherState CurrentWeather { get; private set; } = WeatherState.Clear; // 현재 날씨
    [SerializeField] private float dayDuration = 120f; // 하루의 총 길이 (초 단위)
    private float currentTime = 0f; // 현재 시간 (0 ~ dayDuration)
    public TimeState CurrentTimeOfDay { get; private set; } // 현재 시간대
    private int dayCount = 1; // 경과한 일수

    // 저장/로드를 위한 public 프로퍼티들
    public float DayDuration => dayDuration;
    public float CurrentTime => currentTime;
    public int DayCount => dayCount;
    public BGMManager bgmManager;

    private void Start()
    {
        OnWeatherChanged += playRainBGM;
    }
    private void OnDestroy()
    { 
        OnWeatherChanged -= playRainBGM;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayDuration)
        {
            currentTime = 0f; // 하루가 끝나면 시간 초기화
            dayCount++; // 일수 증가
            OnDayPassed?.Invoke(dayCount); // 하루가 지났음을 알림
            if (dayCount % 1 == 0) // 2일마다 날씨 변화
            {
                int weatherType = UnityEngine.Random.Range(0, 2); // 0: 맑음, 1: 비
                CurrentWeather = (WeatherState)weatherType;
                OnWeatherChanged?.Invoke();
            }
            if (dayCount % 7 == 0) // 7일마다 고양이 슬라임 출현 여부 초기화
            {
                gameManager.IsCat = false;
            }
        }
        UpdateTimeState();
    }

    private void UpdateTimeState()
    {
        float timeRatio = currentTime / dayDuration;
        TimeState newTimeState;

        if (timeRatio < 0.25f)
        {
            newTimeState = TimeState.Morning;   // 아침
        }
        else if (timeRatio < 0.5f)
        {
            newTimeState = TimeState.Afternoon; // 오후
        }
        else if (timeRatio < 0.75f)
        {
            newTimeState = TimeState.Evening;   // 저녁
        }
        else
        {
            newTimeState = TimeState.Night;     // 새벽/밤
        }

        // 시간 상태가 실제로 변경되었을 때만 이벤트 발생
        if (CurrentTimeOfDay != newTimeState)
        {
            CurrentTimeOfDay = newTimeState;
            OnTimeChanged?.Invoke();
        }
    }

    // 저장된 데이터 로드
    public void LoadTimeData(float savedCurrentTime, int savedDayCount, int savedTimeOfDay, int savedWeather, float savedDayDuration)
    {
        currentTime = savedCurrentTime;
        dayCount = savedDayCount;
        CurrentTimeOfDay = (TimeState)savedTimeOfDay;
        CurrentWeather = (WeatherState)savedWeather;
        dayDuration = savedDayDuration;

        Debug.Log($"시간 데이터 로드 완료: Day {dayCount}, Time {currentTime:F1}/{dayDuration}, {CurrentTimeOfDay}, {CurrentWeather}");
    }

    public void playRainBGM()
    {
        if (CurrentWeather == WeatherState.Rain)
        {
            // bgm 출력
            bgmManager.PlayBGM();
            

        }
        else
        {
            bgmManager.StopBGM();
        } 

    }
}
