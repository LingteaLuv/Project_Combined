using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : ScriptableObject
{
    public int EnemyID;
    public string Name;
    public float MaxHP;
    public string AtkType;
    public int AtkDamage;
    public float CastTime;
    public float RecoveryFrame;
    public float AtkCoolDown;
    public float AtkSpeed;
    public float AtkRange;
    public float MoveSpeed;
    public float ChaseMoveSpeed;
    public float NightMoveSpeed;
    public float SightRange;
    public float SightAngle;
    public float HearingRange;
    public float PatrolSight;
    public float ChaseSight;
    public float DeactivateHearing;
    public float PatrolRadius;
    public float SearchTime;
    public float LootChance;
    public string EnemyLootID;
    public string EnemyLootGridChanceID;
}
