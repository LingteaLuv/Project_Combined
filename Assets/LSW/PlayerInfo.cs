using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInfo : ScriptableObject
{
    public int PlayerID;
    public float MaxHP;
    public float MaxStamaina;
    public float MaxMoisture;
    public int StaminaRegen;
    public float StaminaCostRun;
    public float StaminaCostJump;
    public float StaminaCostMelee;
    public float SafeFallDistance;
    public float DeadFallDistance;
    public float FallDamage;
    public float HungerDecrease;
    public int HungerBuffThreshold;
    public float HungerBuffAtkSpeed;
    public int HungerDebuffThreshold;
    public float HungerDebuffAtkSpeed;
    public float MoistureDecrease;
    public int MoistureBuffThreshold;
    public float MoistureBuffMoveSpeed;
    public int MoistureDebuffThreshold;
    public float MoistureDebuffMoveSpeed;
    public float DepletionHp;
    public float HPRegen;
    public float HungerAndMoisture;
    public float MoveSpeed;
    public float MoveNoise;
    public float RunSpeed;
    public float RunNoise;
    public float CrouchSpeed;
    public float CrouchNoise;
    public AudioClip RunSFX;
    public AudioClip StaminaDepletionSFX;
    public AudioClip AtkSFX;
    public int AtkSFXCooldown;
    public AudioClip JumpSFX;
    public AudioClip HitSFX;
    public int HitSFXCooldown;
    public AudioClip DestroyEquipmentSFX;
}
