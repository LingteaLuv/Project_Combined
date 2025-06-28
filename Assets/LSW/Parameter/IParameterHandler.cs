using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParameterHandler
{
    public void ApplyModifier(StatModifier modifier);
    public void RemoveModifier(StatModifier modifier);
}
