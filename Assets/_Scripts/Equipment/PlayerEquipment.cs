using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public PlayerManager Unit;
    public StatsManager playerStats;

    public List<Weapon> Weapons = new List<Weapon>();
    private int _numberOfChildren;

    private float _totalBaseDamage;
    private float _totalWeaponKB;

    #region Weapon Stats
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

    //public System.Action CalculateStats;
    public int NumberOfChildren 
    { 
        get => _numberOfChildren; 
        set
        {
            if (value == _numberOfChildren) return;
            _numberOfChildren = value;
            //_listWeaponsInEquipment();
            //CalculateWeaponBasedStats();
        }
    }

    public float TotalBaseDamage 
    { 
        get => _totalBaseDamage;
        set 
        { 
            _totalBaseDamage = value;
            //CalculateWeaponBasedStats();
        } 
    }
    public float TotalWeaponKB 
    { 
        get => _totalWeaponKB;
        set 
        { 
            _totalWeaponKB = value;
            //CalculateWeaponBasedStats();
        }
    }


    private void OnEnable()
    {
        //playerStats.CalculateStats += _calculateTotalStats;
    }

    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        Unit = GetComponentInParent<PlayerManager>();
        playerStats = Unit.UnitStats;
    }
    void Start()
    {
        //StartCoroutine(getNumberOfChildren(1));
    }

    void Update()
    {
        
    }

    private void _listWeaponsInEquipment()
    {
        // this code will create a list of the weapons listed in equipment
        Weapons.Clear();
        for(int i = 0; i < NumberOfChildren; i++)
        {
            var weapon = transform.GetChild(i).GetComponent<Weapon>();
            if (weapon != null) Weapons.Add(weapon);
        }
    }


    public void CalculateWeaponBasedStats()
    {
        //playerStats.CalculateCurrentWeaponDamage(TotalCurrentWeaponDamage);
        //playerStats.CalculateTrueKnockback(TotalWeaponKB);
        //_getTotalWeaponBaseDamage();

        //CalculateStats?.Invoke();
    }

    //public void SetCurrentWeaponBaseDamage(float baseDamage)
    //{

    //    TotalCurrentWeaponDamage = baseDamage;
    //}

    private float _getTotalWeaponBaseDamage()
    {
        TotalBaseDamage = 0;

        foreach(Weapon weapon in Weapons)
        {
            TotalBaseDamage += weapon.BaseWeaponDamage;
        }
        return TotalBaseDamage;
    }

    public void GetWeaponKnocback(float weaponKB)
    {
        TotalWeaponKB = weaponKB;
    }

    IEnumerator getNumberOfChildren(float numOfSecondsUntilUpdate)
    {
        yield return new WaitForSeconds(numOfSecondsUntilUpdate);
        NumberOfChildren = transform.childCount;

        StartCoroutine(getNumberOfChildren(numOfSecondsUntilUpdate));
    }

    public void UpdateEquipmentStats()
    {
        _clearEquipmentStats();
        _getEquipmentStats();
        Unit.UpdateStats();
    }

    private void _getEquipmentStats()
    {
        // loop to get all stats from the weapon/gear

        foreach (Weapon weapon in Weapons)
        {
            // BONUS STATS
            TotalBonusMaxHealth = weapon.TotalBonusMaxHealth;
            TotalBonusMaxMana = weapon.TotalBonusMaxMana;
            TotalBonusAggro = weapon.TotalBonusAggro;
            TotalBonusAttackSpeed = weapon.TotalBonusAttackSpeed;
            TotalBonusCritHitChance = weapon.TotalBonusCritHitChance;
            TotalBonusDamage = weapon.TotalBonusDamage;
            TotalBonusDefense = weapon.TotalBonusDamage;
            TotalBonusHealthRegen = weapon.TotalBonusHealthRegen;
            TotalBonusKnockback = weapon.TotalBonusKnockback;
            TotalBonusKnockbackResistance = weapon.TotalBonusKnockbackResistance;
            TotalBonusMoveSpeed = weapon.TotalBonusMoveSpeed;

            // PENALTY STATS
            TotalPenaltyMaxHealth = weapon.TotalPenaltyMaxHealth;
            TotalPenaltyMaxMana = weapon.TotalPenaltyMaxMana;
            TotalPenaltyAggro = weapon.TotalPenaltyAggro;
            TotalPenaltyAttackSpeed = weapon.TotalPenaltyAttackSpeed;
            TotalPenaltyCritHitChance = weapon.TotalPenaltyCritHitChance;
            TotalPenaltyDamage = weapon.TotalPenaltyDamage;
            TotalPenaltyDefense = weapon.TotalPenaltyDefense;
            TotalPenaltyHealthRegen = weapon.TotalPenaltyHealthRegen;
            TotalPenaltyKnockback = weapon.TotalPenaltyKnockback;
            TotalPenaltyKnockbackResistance = weapon.TotalPenaltyKnockbackResistance;
            TotalPenaltyMoveSpeed = weapon.TotalPenaltyMoveSpeed;
        }
    }

    private void _clearEquipmentStats()
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
}
