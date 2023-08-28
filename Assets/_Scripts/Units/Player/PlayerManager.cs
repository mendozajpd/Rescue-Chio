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
        SetDefaultStats("Units/Player/PlayerDefaultStats");
        UnitEquipment = GetComponentInChildren<PlayerEquipment>();
        UnitController = GetComponent<PlayerController>();
    }

    private void Start()
    {
    }

    public override void CalculateTotalStats()
    {
        _clearTotalStats();
        _addAllStats();
    }

    protected override void _addAllStats()
    {
        UnitWeaponDefaultDamage = UnitEquipment.CurrentWeaponBaseDamage;
        UnitWeaponDefaultKnockback = UnitEquipment.CurrentWeaponKnockback;
        UnitWeaponDefaultAttackSpeed = UnitEquipment.CurrentWeaponAttackSpeed;

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

        base._addAllStats();
    }

}
