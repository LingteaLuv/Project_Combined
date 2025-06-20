using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property<T>
{
    private T _value;

    public T Value
    {
        get { return _value; }
        set
        {
            // 변경된 값이 기존의 값과 일치하지 않는 경우에만
            if(!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value; 
                OnChanged?.Invoke(_value); 
            }
        }
    }
    public event Action<T> OnChanged;

    public Property(T value)
    {
        _value = value;
    }
}
