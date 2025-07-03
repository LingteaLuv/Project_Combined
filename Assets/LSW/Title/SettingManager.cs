using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    public Property<float> Brightness;
    public Property<float> FOV;
    public Property<float> BGMSound;
    public Property<float> SFXSound;
    public Property<float> MouseXSpeed;
    public Property<float> MouseYSpeed;

    protected override void Awake()
    {
        base.Awake();
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        Init();
    }

    public void SetBrightness(float input)
    {
        Brightness.Value = input;
    }

    public void SetFOV(float input)
    {
        FOV.Value = input;
    }

    public void SetBGMSound(float input)
    {
        BGMSound.Value = input;
    }
    
    public void SetSFXSound(float input)
    {
        SFXSound.Value = input;
    }

    public void SetMouseXSpeed(float input)
    {
        MouseXSpeed.Value = input;
    }
    
    public void SetMouseYSpeed(float input)
    {
        MouseYSpeed.Value = input;
    }
    private void Init()
    {
        Brightness = new Property<float>(0.5f);
        FOV = new Property<float>(0.5f);
        BGMSound = new Property<float>(0.5f);
        SFXSound = new Property<float>(0.5f);
        MouseXSpeed = new Property<float>(0.5f);
        MouseYSpeed = new Property<float>(0.5f);
    }
}
