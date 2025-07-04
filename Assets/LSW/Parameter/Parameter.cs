using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter
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
        StateUpdate();
    }

    private void StateUpdate()
    {
        if (_value > 90)
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

    public virtual void Act() { }
    
    public virtual void Act(ref float stat, float baseValue, float buffOffset, float deBuffOffset) { }

    public void Recover(float value)
    {
        Value = Value + value > 100 ? 100 : Value + value;
    }
    public void Decrease(float value)
    {
        Value = Value - value < 0 ? 0 : Value - value;
    }

    public virtual void Penalty(ref float stat, float baseValue, float buffValue) { }
    public virtual void Penalty() { }
    public virtual void ResetValue(ref float stat, float baseValue) { }
    public virtual void ResetValue() { }
    public virtual void Advantage(ref float stat, float baseValue, float deBuffValue) { }
    public virtual void Advantage() { }
}

public enum ParamState
{
    Full, Basic, Lack, Depletion 
}
