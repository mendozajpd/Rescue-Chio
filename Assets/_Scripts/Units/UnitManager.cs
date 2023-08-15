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


    public List<float> StatList = new List<float>();

    #region STATS
    // BONUS STATS
    public float TotalBonusMaxHealth { get; protected set; }
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
            UnitHealth.SetMaxValue(UnitDefaultStats);


            if(UnitDefaultStats.DebugOn) Debug.Log(gameObject.name + ": " + UnitDefaultStats.name + " : Default Stats Set!");
        } else
        {
            Debug.Log("Default Stats for " + gameObject.name + " not found!");
        }
    }

    public virtual void CalculateBonusPenaltyStats()
    {
        UnitStatusEffects.CalculateStats();
        UnitPowerups.CalculateStats();
    }

    public virtual void UpdateStats()
    {
        StatUpdate.Invoke();
    }
}
