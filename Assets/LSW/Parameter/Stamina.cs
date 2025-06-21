using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : Parameter
{
    public override void Recover(float value)
    {
        Value = Value + value > 100 ? 100 : Value + value;
    }

    public override void Decrease(float value)
    {
        Value = Value - value < 0 ? 0 : Value - value;
    }

    public override void Penalty()
    {
        // 강한 행동(달리기, 공격 불가) => bool 타입 변수?
    }
}
