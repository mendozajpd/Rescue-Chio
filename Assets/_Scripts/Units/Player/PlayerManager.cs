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

    private void Awake()
    {
        GetRequiredComponents();
        SetDefaultStats("Player/PlayerDefaultStats");
        UnitEquipment = GetComponentInChildren<PlayerEquipment>();
        UnitController = GetComponent<PlayerController>();
    }

    //public void GetWeaponBaseDamage()
    //{
    //}


}
