using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;

public class ParameterHudUI : MonoBehaviour
{
    [SerializeField] private ProgressBar _hp;
    [SerializeField] private ProgressBar _hunger;
    [SerializeField] private ProgressBar _thirsty;
    [SerializeField] private ProgressBar _stamina;

    public void HpUpdate(float value)
    {
        _hp.currentPercent = value;
    }
    
    public void HungerUpdate(float value)
    {
        _hunger.currentPercent = value;
    }
    
    public void ThirstyUpdate(float value)
    {
        _thirsty.currentPercent = value;
    }
    
    public void StaminaUpdate(float value)
    {
        _stamina.currentPercent = value;
    }
    
    
}
