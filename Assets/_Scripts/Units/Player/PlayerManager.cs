using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Mana))] [RequireComponent(typeof(StatusEffectsManager), typeof(StatsManager))]
public class PlayerManager : UnitManager
{
    // HEALTH -         UnitHealth
    // MANA -           UnitMana
    // STATUS EFFECTS - UnitStatusEffects
    // STATS -          UnitStats
    // DEFAULT STATS -  UnitDefaultStats

    public PlayerEquipment UnitEquipment;

    private void Awake()
    {
        GetRequiredComponents();
        SetDefaultStats("Player/PlayerDefaultStats");
        UnitEquipment = GetComponentInChildren<PlayerEquipment>();
    }

    public void GetWeaponBaseDamage()
    {

    }


}
