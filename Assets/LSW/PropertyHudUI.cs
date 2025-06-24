using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertyHudUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _atkDamage;
    [SerializeField] private TMP_Text _atkSpeed;

    public void SpeedUpdate(float value)
    {
        _speed.text = $"Speed : {value}";
    }
    
    public void AtkDamageUpdate(float value)
    {
        _atkDamage.text = $"AtkDamage : {value}";
    }
    
    public void AtkSpeedUpdate(float value)
    {
        _atkSpeed.text = $"AtkSpeed : {value}";
    }
}
