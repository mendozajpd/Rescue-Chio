using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public PlayerManager Unit;
    public StatsManager playerStats;

    public List<Weapon> Weapons = new List<Weapon>();
    private int _numberOfChildren;

    private float _currentWeaponBaseDamage;
    private float _currentWeaponBaseKnockback;

    #region Weapon Stats
    protected float _totalBonusMaxHealth;
    public float TotalBonusMaxHealth
    {
        get => _totalBonusMaxHealth;
        protected set
        {
            _totalBonusMaxHealth = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusMaxMana;
    public float TotalBonusMaxMana
    {
        get => _totalBonusMaxMana;
        protected set
        {
            _totalBonusMaxMana = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusAggro;
    public float TotalBonusAggro
    {
        get => _totalBonusAggro;
        protected set
        {
            _totalBonusAggro = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusAttackSpeed;
    public float TotalBonusAttackSpeed
    {
        get => _totalBonusAttackSpeed;
        protected set
        {
            _totalBonusAttackSpeed = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusCritHitChance;
    public float TotalBonusCritHitChance
    {
        get => _totalBonusCritHitChance;
        protected set
        {
            _totalBonusCritHitChance = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusDamage;
    public float TotalBonusDamage
    {
        get => _totalBonusDamage;
        protected set
        {
            _totalBonusDamage = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusDefense;
    public float TotalBonusDefense
    {
        get => _totalBonusDefense;
        protected set
        {
            _totalBonusDefense = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusHealthRegen;
    public float TotalBonusHealthRegen
    {
        get => _totalBonusHealthRegen;
        protected set
        {
            _totalBonusHealthRegen = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusKnockback;
    public float TotalBonusKnockback
    {
        get => _totalBonusKnockback;
        protected set
        {
            _totalBonusKnockback = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusKnockbackResistance;
    public float TotalBonusKnockbackResistance
    {
        get => _totalBonusKnockbackResistance;
        protected set
        {
            _totalBonusKnockbackResistance = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalBonusMoveSpeed;
    public float TotalBonusMoveSpeed
    {
        get => _totalBonusMoveSpeed;
        protected set
        {
            _totalBonusMoveSpeed = value;
            Unit.UpdateStats();
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
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyMaxMana;
    public float TotalPenaltyMaxMana
    {
        get => _totalPenaltyMaxMana;
        protected set
        {
            _totalPenaltyMaxMana = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyAggro;
    public float TotalPenaltyAggro
    {
        get => _totalPenaltyAggro;
        protected set
        {
            _totalPenaltyAggro = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyAttackSpeed;
    public float TotalPenaltyAttackSpeed
    {
        get => _totalPenaltyAttackSpeed;
        protected set
        {
            _totalPenaltyAttackSpeed = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyCritHitChance;
    public float TotalPenaltyCritHitChance
    {
        get => _totalPenaltyCritHitChance;
        protected set
        {
            _totalPenaltyCritHitChance = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyDamage;
    public float TotalPenaltyDamage
    {
        get => _totalPenaltyDamage;
        protected set
        {
            _totalPenaltyDamage = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyDefense;
    public float TotalPenaltyDefense
    {
        get => _totalPenaltyDefense;
        protected set
        {
            _totalPenaltyDefense = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyHealthRegen;
    public float TotalPenaltyHealthRegen
    {
        get => _totalPenaltyHealthRegen;
        protected set
        {
            _totalPenaltyHealthRegen = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyKnockback;
    public float TotalPenaltyKnockback
    {
        get => _totalPenaltyKnockback;
        protected set
        {
            _totalPenaltyKnockback = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyKnockbackResistance;
    public float TotalPenaltyKnockbackResistance
    {
        get => _totalPenaltyKnockbackResistance;
        protected set
        {
            _totalPenaltyKnockbackResistance = value;
            Unit.UpdateStats();
        }
    }

    protected float _totalPenaltyMoveSpeed;
    public float TotalPenaltyMoveSpeed
    {
        get => _totalPenaltyMoveSpeed;
        protected set
        {
            _totalPenaltyMoveSpeed = value;
            Unit.UpdateStats();
        }
    }
    #endregion

    //public System.Action CalculateStats;
    public int NumberOfChildren 
    { 
        get => _numberOfChildren; 
        set
        {
            if (value == _numberOfChildren) return;
            _numberOfChildren = value;
            _listWeaponsInEquipment();
            //CalculateWeaponBasedStats();
        }
    }

    public float CurrentWeaponBaseDamage 
    { 
        get => _currentWeaponBaseDamage;
        set 
        { 
            _currentWeaponBaseDamage = value;
            //CalculateWeaponBasedStats();
        } 
    }
    public float CurrentWeaponKnockback 
    { 
        get => _currentWeaponBaseKnockback;
        set 
        { 
            _currentWeaponBaseKnockback = value;
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
        StartCoroutine(getNumberOfChildren(.1f));
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
        _getEquipmentStats();
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
        CurrentWeaponBaseDamage = 0;

        foreach(Weapon weapon in Weapons)
        {
            CurrentWeaponBaseDamage += weapon.WeaponBaseDamage;
        }
        return CurrentWeaponBaseDamage;
    }

    public void GetWeaponKnocback(float weaponKB)
    {
        CurrentWeaponKnockback = weaponKB;
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
        foreach (Weapon weapon in Weapons) // GETTING ALL WEAPON STATS DOESN'T ACTUALLY MAKE MUCH SENSE
        {
            // BONUS STATS
            CurrentWeaponBaseDamage = weapon.WeaponBaseDamage;
            CurrentWeaponKnockback = weapon.WeaponBaseKnockback;
            TotalBonusMaxHealth = weapon.TotalBonusMaxHealth;
            TotalBonusMaxMana = weapon.TotalBonusMaxMana;
            TotalBonusAggro = weapon.TotalBonusAggro;
            TotalBonusAttackSpeed = weapon.TotalBonusAttackSpeed;
            TotalBonusCritHitChance = weapon.TotalBonusCritHitChance;
            TotalBonusDamage = weapon.TotalBonusDamage;
            TotalBonusDefense = weapon.TotalBonusDefense;
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
