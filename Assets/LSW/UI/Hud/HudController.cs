using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class HudController : MonoBehaviour
{
     [SerializeField] private PropertyHudUI _propertyHudUI;
     [SerializeField] private ParameterHudUI _parameterHudUI;
     
    private PlayerProperty _property;
    
    private void HpUpdate(float value)
    {
        _parameterHudUI.HpUpdate(value);
    }

    private void HungerUpdate(float value)
    {
        _parameterHudUI.HungerUpdate(value);
    }

    private void ThirstyUpdate(float value)
    {
        _parameterHudUI.ThirstyUpdate(value);
    }
    
    private void StaminaUpdate(float value)
    {
        _parameterHudUI.StaminaUpdate(value);
    }
    
    /*private void SpeedUpdate(float value)
    {
        _propertyHudUI.SpeedUpdate(value);
    }
    
    private void AtkDamageUpdate(float value)
    {
        _propertyHudUI.AtkDamageUpdate(value);
    }
    
    private void AtkSpeedUpdate(float value)
    {
        _propertyHudUI.AtkSpeedUpdate(value);
    }*/

    public void Init(PlayerProperty playerProperty)
    {
        _property = playerProperty;
        
        _property.Hp.OnChanged += HpUpdate;
        _property.Hunger.OnChanged += HungerUpdate;
        _property.Thirsty.OnChanged += ThirstyUpdate;
        _property.Stamina.OnChanged += StaminaUpdate;

        //_property.MoveSpeed.OnChanged += SpeedUpdate;
        //_property.AtkDamage.OnChanged += AtkDamageUpdate;
        //_property.AtkSpeed.OnChanged += AtkSpeedUpdate;
        
        HpUpdate(_property.Hp.Value);
        HungerUpdate(_property.Hunger.Value);
        ThirstyUpdate(_property.Thirsty.Value);
        StaminaUpdate(_property.Stamina.Value);

        //SpeedUpdate(_property.MoveSpeed.Value);
        //AtkDamageUpdate(_property.AtkDamage.Value);
        //AtkSpeedUpdate(_property.AtkSpeed.Value);
    }
}
