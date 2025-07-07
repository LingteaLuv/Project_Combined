using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour
{
    [SerializeField] private Image dayImageUI;
    [SerializeField] private Sprite[] daySprites;

    private void Start()
    {
        UpdateDayImage(TimeManager.Instance.DayCount);
        TimeManager.Instance.OnDayChanged += UpdateDayImage;
    }
    private void OnDestroy()
    {
        TimeManager.Instance.OnDayChanged -= UpdateDayImage;
    }
    public void UpdateDayImage(int day)
    {
        int index = Mathf.Clamp(day - 1, 0, daySprites.Length - 1);
        dayImageUI.sprite = daySprites[index];
    }
}
