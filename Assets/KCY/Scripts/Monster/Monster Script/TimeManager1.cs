using System;
using UnityEngine;

/// <summary>
/// 게임 내 시간 흐름을 관리하고, 낮/밤 상태를 판별 및 전환하는 매니저 클래스.
/// </summary>
public class TimeManager1 : Singleton<TimeManager1>
{
    #region Public Properties
    public Property<DayTime> CurrentTimeOfDay { get; private set; } = new Property<DayTime>(DayTime.None);   //  초기 열거형 값
    public Property<int> CurrentHour { get; private set; } = new Property<int>(6);                           //  초기 시간값
    public Property<float> CurrentMinute { get; private set; } = new Property<float>(0f);                    //  초기 분값
    #endregion

    #region Private Properties
    #endregion

    #region Serialized Fields

    [Header("Time Settings")]
    [Tooltip("게임 내 하루가 몇 분 동안 지속되는지 설정합니다.")]
    [SerializeField] private float _dayDurationInMinutes = 0.6667f;  // 하루 40초 → 10초마다 DayTime 변화

    [Tooltip("하루를 몇 시간으로 구성할지 설정합니다.")]
    [SerializeField] private int _hoursPerDay = 24; // 24시간 기준

    [Header("Thresholds")]
    [Tooltip("아침이 시작되는 시각입니다.")]
    [SerializeField] private int _morningStartHour = 6;
    [Tooltip("낮이 시작되는 시각입니다.")]
    [SerializeField] private int _dayStartHour = 12;
    [Tooltip("밤이 시작되는 시각입니다.")]
    [SerializeField] private int _nightStartHour = 18;
    [Tooltip("심야가 시작되는 시각입니다.")]
    [SerializeField] private int _midnightStartHour = 0;

    #endregion

    #region Unity MonoBehavior
    private void Start()
    {
        //Debug.Log($"TimeManager1 실제 인스턴스: {this.GetInstanceID()}");

        // 시간대 변경 시 디버그 로그 출력 (테스트용)
        CurrentTimeOfDay.OnChanged += time =>
        {
            //Debug.Log($"[TimeManager] 시간대 변경됨 → {time}");
        };
    }

    private void Update()
    {
        //Debug.Log($"현재 시각: {CurrentHour.Value}:{(int)CurrentMinute.Value}, 상태: {CurrentTimeOfDay.Value}");
        UpdateGameTime();
        UpdateDayNightCycle();
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// 게임 내 시간을 실시간(하루 40초)에 맞게 증가시킵니다.
    /// </summary>
    private void UpdateGameTime()
    {
        float minutesPerSecond = (_hoursPerDay * 60f) / (_dayDurationInMinutes * 60f);
        CurrentMinute.Value += Time.deltaTime * minutesPerSecond;

        if (CurrentMinute.Value >= 60f)
        {
            CurrentMinute.Value -= 60f;
            CurrentHour.Value++;

            if (CurrentHour.Value >= _hoursPerDay)
                CurrentHour.Value = 0;
        }
    }

    /// <summary>
    /// 현재 시간이 어떤 상황인지 판별하고 변경되면 이벤트 발생. (아침/낮/밤/심야 전환)
    /// </summary>
    private void UpdateDayNightCycle()
    {
        DayTime newTimeOfDay;

        int hour = CurrentHour.Value;

        if (hour >= 0 && hour < 6)
            newTimeOfDay = DayTime.MidNight;
        else if (hour >= 6 && hour < 12)
            newTimeOfDay = DayTime.Morning;
        else if (hour >= 12 && hour < 18)
            newTimeOfDay = DayTime.Day;
        else
            newTimeOfDay = DayTime.Night;

        if (CurrentTimeOfDay.Value != newTimeOfDay)
        {
            CurrentTimeOfDay.Value = newTimeOfDay;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 게임에서의 현재 시각을 HH:MM 형식의 문자열로 반환합니다.
    /// </summary>
    public string GetFormattedTime()
    {
        return $"{CurrentHour.Value:00}:{(int)CurrentMinute.Value:00}";
    }

    #endregion
}
