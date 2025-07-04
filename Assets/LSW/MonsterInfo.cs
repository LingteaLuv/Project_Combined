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
    public float AtkCoolDown;
    public float AtkSpeed;
    public float AtkRange;
    public float CastTime;
    public float RecoveryFrame;
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
    public string EnemyLootID;
    public string EnemyLootGridChanceID;
    public string IdleSfx; // idle모드 사운드
    public string IdleSfxRange; // idle모드 사운드 범위
    public string AtkSfx; // 1회 공격 사운드
    public string HitSfx; // 1회 피격 사운드
    public string DieSfx; // 사망 사운드
    public float NightAtkCoolDown; // 밤 일때의 공격 쿨타임  
    public float NightChaseMoveSpeed; // 밤동안의 추격 속도
}
