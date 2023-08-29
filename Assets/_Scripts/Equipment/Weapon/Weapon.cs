using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    [Header("Common Weapon Stats")]
    private float _weaponBaseDamage;
    private float _weaponBaseKnockback;
    private float _weaponBaseAttackSpeed;

    public float WeaponBaseDamage
    {
        get => _weaponBaseDamage;
        protected set
        {
            _weaponBaseDamage = value;
            equipment?.UpdateEquipmentStats();
        }
    }
    public float WeaponBaseKnockback
    {
        get => _weaponBaseKnockback;
        protected set
        {
            _weaponBaseKnockback = value;
            equipment?.UpdateEquipmentStats();
        }
    }
    public float WeaponBaseAttackSpeed
    {
        get => _weaponBaseAttackSpeed;
        set
        {
            _weaponBaseAttackSpeed = value;
            equipment?.UpdateEquipmentStats();
        }
    }


    protected float useTime;
    public float UseTimeDuration;

    #region Weapon Stats

    protected float _totalBonusMaxHealth;
    public float TotalBonusMaxHealth
    {
        get => _totalBonusMaxHealth;
        set
        {
            _totalBonusMaxHealth = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusMaxMana;
    public float TotalBonusMaxMana
    {
        get => _totalBonusMaxMana;
        set
        {
            _totalBonusMaxMana = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusAggro;
    public float TotalBonusAggro
    {
        get => _totalBonusAggro;
        set
        {
            _totalBonusAggro = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusAttackSpeed;
    public float TotalBonusAttackSpeed
    {
        get => _totalBonusAttackSpeed;
        set
        {
            _totalBonusAttackSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusCritHitChance;
    public float TotalBonusCritHitChance
    {
        get => _totalBonusCritHitChance;
        set
        {
            _totalBonusCritHitChance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusDamage;
    public float TotalBonusDamage
    {
        get => _totalBonusDamage;
        set
        {
            _totalBonusDamage = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusDefense;
    public float TotalBonusDefense
    {
        get => _totalBonusDefense;
        set
        {
            _totalBonusDefense = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusKnockback;
    public float TotalBonusKnockback
    {
        get => _totalBonusKnockback;
        set
        {
            _totalBonusKnockback = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusKnockbackResistance;
    public float TotalBonusKnockbackResistance
    {
        get => _totalBonusKnockbackResistance;
        set
        {
            _totalBonusKnockbackResistance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalBonusMoveSpeed;
    public float TotalBonusMoveSpeed
    {
        get => _totalBonusMoveSpeed;
        set
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
        set
        {
            _totalPenaltyMaxHealth = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyMaxMana;
    public float TotalPenaltyMaxMana
    {
        get => _totalPenaltyMaxMana;
        set
        {
            _totalPenaltyMaxMana = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyAggro;
    public float TotalPenaltyAggro
    {
        get => _totalPenaltyAggro;
        set
        {
            _totalPenaltyAggro = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyAttackSpeed;
    public float TotalPenaltyAttackSpeed
    {
        get => _totalPenaltyAttackSpeed;
        set
        {
            _totalPenaltyAttackSpeed = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyCritHitChance;
    public float TotalPenaltyCritHitChance
    {
        get => _totalPenaltyCritHitChance;
        set
        {
            _totalPenaltyCritHitChance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyDamage;
    public float TotalPenaltyDamage
    {
        get => _totalPenaltyDamage;
        set
        {
            _totalPenaltyDamage = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyDefense;
    public float TotalPenaltyDefense
    {
        get => _totalPenaltyDefense;
        set
        {
            _totalPenaltyDefense = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyKnockback;
    public float TotalPenaltyKnockback
    {
        get => _totalPenaltyKnockback;
        set
        {
            _totalPenaltyKnockback = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyKnockbackResistance;
    public float TotalPenaltyKnockbackResistance
    {
        get => _totalPenaltyKnockbackResistance;
        set
        {
            _totalPenaltyKnockbackResistance = value;
            equipment.UpdateEquipmentStats();
        }
    }

    protected float _totalPenaltyMoveSpeed;
    public float TotalPenaltyMoveSpeed
    {
        get => _totalPenaltyMoveSpeed;
        set
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

    // Autofire
    protected System.Action UseWeapon;
    protected bool Autofire;


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


    #region Usetimer / Autofire
    protected virtual void UseTimer(float atkSpeed)
    {
        if (useTime > 0)
        {
            useTime -= Time.deltaTime + (atkSpeed * 0.0025f);
        }

        if (useTime <= 0)
        {
            InvokeAutoFire();
        }
    }

    protected void ActivateAutoFire(System.Action attack)
    {
        HoldFire.Enable();
        HoldFire.started += StartAutoFire;
        HoldFire.canceled += StopAutoFire;
        UseWeapon += attack;
    }

    protected void DisableAutoFire(System.Action attack)
    {
        HoldFire.started -= StartAutoFire;
        HoldFire.canceled -= StopAutoFire;
        UseWeapon -= attack;
        HoldFire.Disable();
    }

    protected void StartAutoFire(InputAction.CallbackContext context)
    {
        Autofire = true;
        //Debug.Log("Autofire activated!");
    }

    protected void StopAutoFire(InputAction.CallbackContext context)
    {
        Autofire = false;
        //Debug.Log("Stopped Autofire!");
    }

    protected void InvokeAutoFire()
    {
        if (Autofire) UseWeapon?.Invoke();
    }
    #endregion

}
