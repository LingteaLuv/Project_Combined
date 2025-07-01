using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    public Property<float> Brightness;
    public Property<float> FOV;
    public Property<float> Sound;
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

    public void SetSound(float input)
    {
        Sound.Value = input;
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
        Sound = new Property<float>(0.5f);
        MouseXSpeed = new Property<float>(0.5f);
        MouseYSpeed = new Property<float>(0.5f);
    }
}
