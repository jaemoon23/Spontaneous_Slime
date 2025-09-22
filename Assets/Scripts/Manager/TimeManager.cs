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
    public static event Action<int> OnDayPassed;
    [SerializeField] private float dayDuration = 120f; // 하루의 총 길이 (초 단위)
    private float currentTime = 0f; // 현재 시간 (0 ~ dayDuration)
    public TimeState CurrentTimeOfDay { get; private set; } // 현재 시간대
    private int dayCount = 1; // 경과한 일수
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayDuration)
        {
            currentTime = 0f; // 하루가 끝나면 시간 초기화
            dayCount++; // 일수 증가
            OnDayPassed?.Invoke(dayCount); // 하루가 지났음을 알림
        }
        UpdateTimeState();
    }

    private void UpdateTimeState()
    {
        float timeRatio = currentTime / dayDuration;

        if (timeRatio < 0.25f)
        {
            CurrentTimeOfDay = TimeState.Morning;   // 아침
        }
        else if (timeRatio < 0.5f)
        {
            CurrentTimeOfDay = TimeState.Afternoon; // 오후
        }
        else if (timeRatio < 0.75f)
        {
            CurrentTimeOfDay = TimeState.Evening;   // 저녁
        }
        else
        {
            CurrentTimeOfDay = TimeState.Night;     // 새벽/밤
        }
    }
}
