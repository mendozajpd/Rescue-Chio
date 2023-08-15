using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    // Common Weapon Variables
    public float TotalWeaponDamage { get; protected set; }
    public float BaseWeaponDamage { get; protected set; }
    public float TotalWeaponKnockback { get; protected set; }
    public float BaseWeaponKnockback { get; protected set; }
    public float UseTime;



    #region Weapon Stats
    // BONUS STATS
    public float TotalBonusMaxHealth 
    {
        get => TotalBonusMaxHealth;
        protected set
        {
            TotalBonusMaxHealth = value;
            equipment.UpdateEquipmentStats();
        } 
    }
    public float TotalBonusMaxMana { get; protected set; }
    public float TotalBonusAggro { get; protected set; }
    public float TotalBonusAttackSpeed { get; protected set; }
    public float TotalBonusCritHitChance { get; protected set; }
    public float TotalBonusDamage { get; protected set; }
    public float TotalBonusDefense { get; protected set; }
    public float TotalBonusHealthRegen { get; protected set; }
    public float TotalBonusKnockback { get; protected set; }
    public float TotalBonusKnockbackResistance { get; protected set; }
    public float TotalBonusMoveSpeed { get; protected set; }

    // PENALTY STATS
    public float TotalPenaltyMaxHealth { get; protected set; }
    public float TotalPenaltyMaxMana { get; protected set; }
    public float TotalPenaltyAggro { get; protected set; }
    public float TotalPenaltyAttackSpeed { get; protected set; }
    public float TotalPenaltyCritHitChance { get; protected set; }
    public float TotalPenaltyDamage { get; protected set; }
    public float TotalPenaltyDefense { get; protected set; }
    public float TotalPenaltyHealthRegen { get; protected set; }
    public float TotalPenaltyKnockback { get; protected set; }
    public float TotalPenaltyKnockbackResistance { get; protected set; }
    public float TotalPenaltyMoveSpeed { get; protected set; }
    #endregion

    //Input System Variables
    public PlayerInputActions playerControls;
    public InputAction Fire;
    public InputAction ChangeAttack;
    public InputAction HoldFire;

    // Weapon Sprite
    public SpriteRenderer Sprite;
    public Animator Anim;

    // Equipment
    public PlayerEquipment equipment;

    public void UseTimer()
    {
        if (UseTime > 0)
        {
            UseTime -= Time.deltaTime;
        }
    }

    protected void SetInputVariables()
    {
        playerControls = new PlayerInputActions();
        Fire = playerControls.Player.Fire;
        ChangeAttack = playerControls.Player.ChangeAttack;
        HoldFire = playerControls.Player.HoldFire;
    }

    protected void SetSpriteVariables()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
    }


    protected void SetTotalWeaponDamage(PlayerEquipment equipment)
    {
        TotalWeaponDamage = equipment.playerStats.CalculateTrueDamage(BaseWeaponDamage);
    }

    protected void SetTotalWeaponKnockback(PlayerEquipment equipment)
    {
        TotalWeaponKnockback = equipment.playerStats.CalculateTrueKnockback(BaseWeaponKnockback);
    }

}
