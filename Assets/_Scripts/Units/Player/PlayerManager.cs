using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Mana))]
[RequireComponent(typeof(StatusEffectsManager), typeof(StatsManager))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : UnitManager
{
    // HEALTH -         UnitHealth
    // MANA -           UnitMana
    // STATUS EFFECTS - UnitStatusEffects
    // STATS -          UnitStats
    // DEFAULT STATS -  UnitDefaultStats

    public PlayerEquipment UnitEquipment;
    public PlayerController UnitController;


    private void Awake()
    {
        GetRequiredComponents();
        SetDefaultStats("Player/PlayerDefaultStats");
        UnitEquipment = GetComponentInChildren<PlayerEquipment>();
        UnitController = GetComponent<PlayerController>();
        //UnitEquipment.CalculateStats += CalculateBonusPenaltyStats;
    }

    private void Start()
    {
        //_overwriteStatlist();
    }

    private void _clearTotalStats()
    {
        // BONUS STATS
        TotalBonusMaxHealth = 0;
        TotalBonusMaxMana = 0;
        TotalBonusAggro = 0;
        TotalBonusAttackSpeed = 0;
        TotalBonusCritHitChance = 0;
        TotalBonusDamage = 0;
        TotalBonusDefense = 0;
        TotalBonusHealthRegen = 0;
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
        TotalPenaltyHealthRegen = 0;
        TotalPenaltyKnockback = 0;
        TotalPenaltyKnockbackResistance = 0;
        TotalPenaltyMoveSpeed = 0;
    }

    public override void CalculateBonusPenaltyStats()
    {
        base.CalculateBonusPenaltyStats();
        _clearTotalStats();
        _addAllStats();
    }

    private void _addAllStats()
    {
        // Total stats = equipment stats + Powerups stats + status effects stats
        UnitBaseDamage = UnitEquipment.CurrentWeaponBaseDamage;
        UnitBaseKnockback = UnitEquipment.CurrentWeaponKnockback;

        TotalBonusMaxHealth = UnitEquipment.TotalBonusMaxHealth;
        TotalBonusMaxMana = UnitEquipment.TotalBonusMaxMana;
        TotalBonusAggro = UnitEquipment.TotalBonusAggro;
        TotalBonusAttackSpeed = UnitEquipment.TotalBonusAttackSpeed;
        TotalBonusCritHitChance = UnitEquipment.TotalBonusCritHitChance;
        TotalBonusDamage = UnitEquipment.TotalBonusDamage;
        TotalBonusDefense = UnitEquipment.TotalBonusDefense;
        TotalBonusHealthRegen = UnitEquipment.TotalBonusHealthRegen;
        TotalBonusKnockback = UnitEquipment.TotalBonusKnockback;
        TotalBonusKnockbackResistance = UnitEquipment.TotalBonusKnockbackResistance;
        TotalBonusMoveSpeed = UnitEquipment.TotalBonusMoveSpeed;

        TotalPenaltyMaxHealth = UnitEquipment.TotalPenaltyMaxHealth;
        TotalPenaltyMaxMana = UnitEquipment.TotalPenaltyMaxMana;
        TotalPenaltyAggro = UnitEquipment.TotalPenaltyAggro;
        TotalPenaltyAttackSpeed = UnitEquipment.TotalPenaltyAttackSpeed;
        TotalPenaltyCritHitChance = UnitEquipment.TotalPenaltyCritHitChance;
        TotalPenaltyDamage = UnitEquipment.TotalPenaltyDamage;
        TotalPenaltyDefense = UnitEquipment.TotalPenaltyDefense;
        TotalPenaltyHealthRegen = UnitEquipment.TotalPenaltyHealthRegen;
        TotalPenaltyKnockback = UnitEquipment.TotalPenaltyKnockback;
        TotalPenaltyKnockbackResistance = UnitEquipment.TotalPenaltyKnockbackResistance;
        TotalPenaltyMoveSpeed = UnitEquipment.TotalPenaltyMoveSpeed;

    }

}
