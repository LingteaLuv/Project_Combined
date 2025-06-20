using System;
using UnityEngine;

/// <summary>
/// ���� �� �ð� �帧�� �����ϰ�, ��/�� ���¸� �Ǻ� �� ��ȯ�ϴ� �Ŵ��� Ŭ����.
/// </summary>
public class TimeManager : Singleton<TimeManager>
{
    #region Public Properties
    public Property<DayTime> CurrentTimeOfDay { get; private set; } = new Property<DayTime>(DayTime.Day);
    public Property<int> CurrentHour { get; private set; } = new Property<int>(6);
    public Property<float> CurrentMinute { get; private set; } = new Property<float>(0f);
    #endregion

    #region Private Properties
    #endregion
    #region Serialized Fields

    [Header("Time Settings")]
    [Tooltip("���� �� �Ϸ簡 �� �� ���� ���ӵǴ��� �����մϴ�.")]
    [SerializeField] private float _dayDurationInMinutes = 10f;

    [Tooltip("�Ϸ縦 �� �ð����� �������� �����մϴ�.")]
    [SerializeField] private int _hoursPerDay = 24;

    [Header("Thresholds")]
    [Tooltip("��ħ�� ���۵Ǵ� �ð��Դϴ�.")]
    [SerializeField] private int _morningStartHour = 6;
    [Tooltip("���� ���۵Ǵ� �ð��Դϴ�.")]
    [SerializeField] private int _dayStartHour = 12;
    [Tooltip("���� ���۵Ǵ� �ð��Դϴ�.")]
    [SerializeField] private int _nightStartHour = 18;
    [Tooltip("�ɾ߰� ���۵Ǵ� �ð��Դϴ�.")]
    [SerializeField] private int _midnightStartHour = 0;

    #endregion

    #region Unity MonoBehavior
    private void Update()
    {
        UpdateGameTime();
        UpdateDayNightCycle();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// ���� �� �ð��� �ǽð�(10�� : 1��)�� �°� ������ŵ�ϴ�.
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
    /// ���� �ð��� � ��Ȳ���� �Ǻ��ϰ� ����Ǹ� �̺�Ʈ �߻�.
    /// </summary>
    private void UpdateDayNightCycle()
    {
        DayTime newTimeOfDay;

        int hour = CurrentHour.Value;

        if (hour >= _midnightStartHour && hour < _morningStartHour)
            newTimeOfDay = DayTime.MidNight;
        else if (hour >= _morningStartHour && hour < _dayStartHour)
            newTimeOfDay = DayTime.Morning;
        else if (hour >= _dayStartHour && hour < _nightStartHour)
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
    /// ���� �ð��� HH:MM ������ ���ڿ��� ��ȯ�մϴ�.
    /// </summary>
    public string GetFormattedTime()
    {
        return $"{CurrentHour.Value:00}:{(int)CurrentMinute.Value:00}";
    }

    #endregion

    #region Test Code
    private void Start()
    {
        CurrentHour.OnChanged += hour =>
        {
            Debug.Log($"[TimeManager] ���� �ð�: {GetFormattedTime()}");
        };
    }
    #endregion
}