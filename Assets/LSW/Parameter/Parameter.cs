using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parameter : MonoBehaviour
{
    private float _value;
    public float Value
    {
        get { return _value; }
        set
        {
            // 변경된 값이 기존의 값과 일치하지 않는 경우에만
            if(!EqualityComparer<float>.Default.Equals(_value, value))
            {
                _value = value;
                StateUpdate();
                OnChanged?.Invoke(_value); 
            }
        }
    }
    public event Action<float> OnChanged;

    public ParamState State { get; private set; }

    public void Init(float value)
    {
        _value = value;
    }

    private void StateUpdate()
    {
        if (_value > 99)
        {
            State = ParamState.Full;
        }
        else if (_value > 40)
        {
            State = ParamState.Basic;
        }
        else if (_value > 10)
        {
            State = ParamState.Lack;
        }
        else
        {
            State = ParamState.Depletion;
        }
    }
    
    public abstract void Recover(float value);
    public abstract void Decrease(float value);

    public virtual void Penalty(ref float stat, float value) { }
    public virtual void Penalty() { }
    
    public virtual void Reset(ref float stat, float value) { }
    public virtual void Reset() { }
    public virtual void Advantage(ref float stat, float value) { }
    public virtual void Advantage() { }
}

public enum ParamState
{
    Full, Basic, Lack, Depletion 
}
