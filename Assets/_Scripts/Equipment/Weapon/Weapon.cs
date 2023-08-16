using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    // Common Weapon Variables
    public float TotalWeaponDamage { get; protected set; }
    public float WeaponBaseDamage { get; protected set; }
    public float TotalWeaponKnockback { get; protected set; }
    public float WeaponBaseKnockback { get; protected set; }
    public float UseTime;



    #region Weapon Stats

    protected float _totalBonusMaxHealth;
    public float TotalBonusMaxHealth
    {
        get => _totalBonusMaxHealth;
        protected set
        {
            _totalBonusMaxHealth = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusMaxMana;
    public float TotalBonusMaxMana
    {
        get => _totalBonusMaxMana;
        protected set
        {
            _totalBonusMaxMana = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusAggro;
    public float TotalBonusAggro
    {
        get => _totalBonusAggro;
        protected set
        {
            _totalBonusAggro = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusAttackSpeed;
    public float TotalBonusAttackSpeed
    {
        get => _totalBonusAttackSpeed;
        protected set
        {
            _totalBonusAttackSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusCritHitChance;
    public float TotalBonusCritHitChance
    {
        get => _totalBonusCritHitChance;
        protected set
        {
            _totalBonusCritHitChance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusDamage;
    public float TotalBonusDamage
    {
        get => _totalBonusDamage;
        protected set
        {
            _totalBonusDamage = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusDefense;
    public float TotalBonusDefense
    {
        get => _totalBonusDefense;
        protected set
        {
            _totalBonusDefense = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusHealthRegen;
    public float TotalBonusHealthRegen
    {
        get => _totalBonusHealthRegen;
        protected set
        {
            _totalBonusHealthRegen = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusKnockback;
    public float TotalBonusKnockback
    {
        get => _totalBonusKnockback;
        protected set
        {
            _totalBonusKnockback = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusKnockbackResistance;
    public float TotalBonusKnockbackResistance
    {
        get => _totalBonusKnockbackResistance;
        protected set
        {
            _totalBonusKnockbackResistance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusMoveSpeed;
    public float TotalBonusMoveSpeed
    {
        get => _totalBonusMoveSpeed;
        protected set
        {
            _totalBonusMoveSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }

    // PENALTY STATS
    protected float _totalPenaltyMaxHealth;
    public float TotalPenaltyMaxHealth
    {
        get => _totalPenaltyMaxHealth;
        protected set
        {
            _totalPenaltyMaxHealth = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyMaxMana;
    public float TotalPenaltyMaxMana
    {
        get => _totalPenaltyMaxMana;
        protected set
        {
            _totalPenaltyMaxMana = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyAggro;
    public float TotalPenaltyAggro
    {
        get => _totalPenaltyAggro;
        protected set
        {
            _totalPenaltyAggro = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyAttackSpeed;
    public float TotalPenaltyAttackSpeed
    {
        get => _totalPenaltyAttackSpeed;
        protected set
        {
            _totalPenaltyAttackSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyCritHitChance;
    public float TotalPenaltyCritHitChance
    {
        get => _totalPenaltyCritHitChance;
        protected set
        {
            _totalPenaltyCritHitChance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyDamage;
    public float TotalPenaltyDamage
    {
        get => _totalPenaltyDamage;
        protected set
        {
            _totalPenaltyDamage = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyDefense;
    public float TotalPenaltyDefense
    {
        get => _totalPenaltyDefense;
        protected set
        {
            _totalPenaltyDefense = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyHealthRegen;
    public float TotalPenaltyHealthRegen
    {
        get => _totalPenaltyHealthRegen;
        protected set
        {
            _totalPenaltyHealthRegen = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyKnockback;
    public float TotalPenaltyKnockback
    {
        get => _totalPenaltyKnockback;
        protected set
        {
            _totalPenaltyKnockback = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyKnockbackResistance;
    public float TotalPenaltyKnockbackResistance
    {
        get => _totalPenaltyKnockbackResistance;
        protected set
        {
            _totalPenaltyKnockbackResistance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyMoveSpeed;
    public float TotalPenaltyMoveSpeed
    {
        get => _totalPenaltyMoveSpeed;
        protected set
        {
            _totalPenaltyMoveSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }
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



}
