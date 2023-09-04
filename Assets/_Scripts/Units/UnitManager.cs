using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitManager : MonoBehaviour
{
    // Health
    public Health UnitHealth;

    // Mana
    public Mana UnitMana;
    // UNIT WILL GET A MANA SCRIPT, BUT MANA WILL ONLY APPEAR IF THE MANA IS MORE THAN 0

    // Status Effects
    public StatusEffectsManager UnitStatusEffects;

    // Stats
    public StatsManager UnitStats;

    // Powerups
    public PowerupsManager UnitPowerups;


    public DefaultStatsSO UnitDefaultStats;


    //public List<float> StatList = new List<float>();

    #region STATS
    // BONUS STATS

    public float UnitWeaponDefaultDamage;
    public float UnitWeaponDefaultKnockback;
    public float UnitWeaponDefaultAttackSpeed;

    public float TotalBonusMaxHealth { get; protected set; }
    public float TotalBonusMaxMana { get; protected set; }
    public float TotalBonusAggro { get; protected set; }
    public float TotalBonusAttackSpeed { get; protected set; }
    public float TotalBonusCritHitChance { get; protected set; }
    public float TotalBonusDamage { get; protected set; }
    public float TotalBonusDefense { get; protected set; }
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
    public float TotalPenaltyKnockback { get; protected set; }
    public float TotalPenaltyKnockbackResistance { get; protected set; }
    public float TotalPenaltyMoveSpeed { get; protected set; }


    #endregion

    public System.Action StatUpdate;

    public void GetRequiredComponents()
    {
        UnitHealth = GetComponent<Health>();
        UnitStatusEffects = GetComponent<StatusEffectsManager>();
        UnitStats = GetComponent<StatsManager>();
        UnitMana = GetComponent<Mana>();
        UnitPowerups = GetComponent<PowerupsManager>();
    }

    public void SetDefaultStats(string location)
    {
        UnitDefaultStats = Resources.Load<DefaultStatsSO>(location);

        if (UnitDefaultStats != null)
        {
            UnitStats.SetDefaultStats(UnitDefaultStats);
            UnitHealth.SetMaxValue(UnitDefaultStats.DefaultMaxHealth);

            if(UnitStats.DebugMode) Debug.Log(gameObject.name + ": " + UnitDefaultStats.name + " : Default Stats Set!");
        } else
        {
            Debug.Log("Default Stats for " + gameObject.name + " not found!");
        }
    }

    protected void _clearTotalStats()
    {
        // BONUS STATS
        TotalBonusMaxHealth = 0;
        TotalBonusMaxMana = 0;
        TotalBonusAggro = 0;
        TotalBonusAttackSpeed = 0;
        TotalBonusCritHitChance = 0;
        TotalBonusDamage = 0;
        TotalBonusDefense = 0;
        TotalBonusKnockback = 0;
        TotalBonusKnockbackResistance = 0;
        TotalBonusMoveSpeed = 0;

        // PENALTY STATS
        TotalPenaltyMaxHealth = 0;
        TotalPenaltyMaxMana = 0;
        TotalPenaltyAggro = 0;
        TotalPenaltyAttackSpeed = 0;
        TotalPenaltyCritHitChance = 0;
        TotalPenaltyDamage = 0;
        TotalPenaltyDefense = 0;
        TotalPenaltyKnockback = 0;
        TotalPenaltyKnockbackResistance = 0;
        TotalPenaltyMoveSpeed = 0;
    }
    
    protected virtual void _addAllStats()
    {
        TotalBonusMaxHealth += UnitPowerups.TotalBonusMaxHealth + UnitStatusEffects.TotalBonusMaxHealth; ;
        TotalBonusMaxMana += UnitPowerups.TotalBonusMaxMana + UnitStatusEffects.TotalBonusMaxMana; ;
        TotalBonusAggro += UnitPowerups.TotalBonusAggro + UnitStatusEffects.TotalBonusAggro;
        TotalBonusAttackSpeed += UnitPowerups.TotalBonusAttackSpeed + UnitStatusEffects.TotalBonusAttackSpeed;
        TotalBonusCritHitChance += UnitPowerups.TotalBonusCritHitChance + UnitStatusEffects.TotalBonusCritHitChance;
        TotalBonusDamage += UnitPowerups.TotalBonusDamage + UnitStatusEffects.TotalBonusDamage;
        TotalBonusDefense += UnitPowerups.TotalBonusDefense + UnitStatusEffects.TotalBonusDefense;
        TotalBonusKnockback += UnitPowerups.TotalBonusKnockback + UnitStatusEffects.TotalBonusKnockback;
        TotalBonusKnockbackResistance += UnitPowerups.TotalBonusKnockbackResistance + UnitStatusEffects.TotalBonusKnockbackResistance; ;
        TotalBonusMoveSpeed += UnitPowerups.TotalBonusMovementSpeed + UnitStatusEffects.TotalBonusMovementSpeed;

        TotalPenaltyMaxHealth += UnitPowerups.TotalPenaltyMaxHealth + UnitStatusEffects.TotalPenaltyMaxHealth; ;
        TotalPenaltyMaxMana += UnitPowerups.TotalPenaltyMaxMana + UnitStatusEffects.TotalPenaltyMaxMana;
        TotalPenaltyAggro += UnitPowerups.TotalPenaltyAggro + UnitStatusEffects.TotalPenaltyAggro;
        TotalPenaltyAttackSpeed += UnitPowerups.TotalPenaltyAttackSpeed + UnitStatusEffects.TotalPenaltyAttackSpeed; ;
        TotalPenaltyCritHitChance += UnitPowerups.TotalPenaltyCritHitChance + UnitStatusEffects.TotalPenaltyCritHitChance;
        TotalPenaltyDamage += UnitPowerups.TotalPenaltyDamage + UnitStatusEffects.TotalPenaltyDamage;
        TotalPenaltyDefense += UnitPowerups.TotalPenaltyDefense + UnitStatusEffects.TotalPenaltyDefense;
        TotalPenaltyKnockback += UnitPowerups.TotalPenaltyKnockback + UnitStatusEffects.TotalPenaltyKnockback;
        TotalPenaltyKnockbackResistance += UnitPowerups.TotalPenaltyKnockbackResistance + UnitStatusEffects.TotalPenaltyKnockbackResistance;
        TotalPenaltyMoveSpeed += UnitPowerups.TotalPenaltyMoveSpeed + UnitStatusEffects.TotalPenaltyMoveSpeed; ;
    }

    public virtual void CalculateTotalStats()
    {
        _clearTotalStats();
        _addAllStats();
    }


    public virtual void UpdateStats()
    {
        StatUpdate.Invoke();
    }

}
