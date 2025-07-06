using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : ScriptableObject
{
    public int EnemyID;
    public string Name;
    public float MaxHP;
    public int AtkDamage;
    public float AtkCoolDown;
    public float AtkRange;
    public float MoveSpeed;
    public float ChaseMoveSpeed;
    public float NightMoveSpeed;
    public float NightChaseMoveSpeed; // 밤동안의 추격 속도
    public float NightAtkCoolDown; // 밤 일때의 공격 쿨타임  
    public float SightRange;
    public float SightAngle;
    public float HearingRange;
    public float DeactivateHearing;
    public float PatrolSight;
    public float ChaseSight;
    public float PatrolRadius;
    public float SearchTime;
    public string EnemyLootID;
    public string EnemyLootGridChanceID;
    public AudioClip IdleSfx1; // idle모드 사운드
    public AudioClip IdleSfx2; // idle모드 사운드
    public float IdleSfxRange; // idle모드 사운드 범위
    public AudioClip ChaseSfx1;
    public AudioClip ChaseSfx2;
    public AudioClip AtkSfx; // 1회 공격 사운드
    public AudioClip HitSfx; // 1회 피격 사운드
    public AudioClip DieSfx; // 사망 사운드
}
