using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Mana))] [RequireComponent(typeof(StatusEffectsManager), typeof(StatsManager))]
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

    // Get other total stats
    private float _totalDamage;
    private float _totalKnockback;

    private void Awake()
    {
        GetRequiredComponents();
        SetDefaultStats("Player/PlayerDefaultStats");
        UnitEquipment = GetComponentInChildren<PlayerEquipment>();
        UnitController = GetComponent<PlayerController>();

    }

    private void _clearTotalStats()
    {
        _totalDamage = 0;
        _totalKnockback = 0;
    }

    public override void CalculateStats()
    {
        // Clear previous total stats
        _clearTotalStats();
        // Total the bonus and penalty stats from equipment/weapons
        _calculateEquipmentStats();
        // Calculate and add total bonus and penalty stats to current total
        base.CalculateStats();
    }
    private void _calculateEquipmentStats()
    {
        _totalDamage = UnitEquipment.TotalBaseDamage;
        _totalKnockback = UnitEquipment.TotalWeaponKB;
    }

}
