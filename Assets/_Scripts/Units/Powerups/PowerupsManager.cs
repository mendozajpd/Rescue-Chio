using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    // Update Method Speed
    [SerializeField] private float updateSpeed = 1; // In seconds
    private float _updateTime;
    // Player Variables

    // Variables for powerups
    public UnitManager Unit;

    #region STATS

    [Header("BONUS STATS")]
    [SerializeField] private float _totalBonusMaxHealth;
    [SerializeField] private float _totalBonusMaxMana;
    [SerializeField] private float _totalBonusAggro;
    [SerializeField] private float _totalBonusAttackSpeed;
    [SerializeField] private float _totalBonusCritHitChance;
    [SerializeField] private float _totalBonusDamage;
    [SerializeField] private float _totalBonusDefense;
    [SerializeField] private float _totalBonusKnockback;
    [SerializeField] private float _totalBonusKnockbackResistance;
    [SerializeField] private float _totalBonusMovementSpeed;

    [Header("PENALTY STATS")]
    [SerializeField] private float _totalPenaltyMaxHealth;
    [SerializeField] private float _totalPenaltyMaxMana;
    [SerializeField] private float _totalPenaltyAggro;
    [SerializeField] private float _totalPenaltyAttackSpeed;
    [SerializeField] private float _totalPenaltyCritHitChance;
    [SerializeField] private float _totalPenaltyDamage;
    [SerializeField] private float _totalPenaltyDefense;
    [SerializeField] private float _totalPenaltyKnockback;
    [SerializeField] private float _totalPenaltyKnockbackResistance;
    [SerializeField] private float _totalPenaltyMoveSpeed;

    // BONUS
    public float TotalBonusMaxHealth
    {
        get => _totalBonusMaxHealth;
        set
        {
            _totalBonusMaxHealth = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusMaxMana
    {
        get => _totalBonusMaxMana;
        set
        {
            _totalBonusMaxMana = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusAggro
    {
        get => _totalBonusAggro;
        set
        {
            _totalBonusAggro = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusAttackSpeed
    {
        get => _totalBonusAttackSpeed;
        set
        {
            _totalBonusAttackSpeed = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusCritHitChance
    {
        get => _totalBonusCritHitChance;
        set
        {
            _totalBonusCritHitChance = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusDamage
    {
        get => _totalBonusDamage;
        set
        {
            _totalBonusDamage = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusDefense
    {
        get => _totalBonusDefense;
        set
        {
            _totalBonusDefense = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusKnockback
    {
        get => _totalBonusKnockback;
        set
        {
            _totalBonusKnockback = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusKnockbackResistance
    {
        get => _totalBonusKnockbackResistance;
        set
        {
            _totalBonusKnockbackResistance = value;
            Unit.UpdateStats();
        }
    }

    public float TotalBonusMovementSpeed
    {
        get => _totalBonusMovementSpeed;
        set
        {
            _totalBonusMovementSpeed = value;
            Unit.UpdateStats();
        }
    }

    // PENALTY
    public float TotalPenaltyMaxHealth
    {
        get => _totalPenaltyMaxHealth;
        set
        {
            _totalPenaltyMaxHealth = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyMaxMana
    {
        get => _totalPenaltyMaxMana;
        set
        {
            _totalPenaltyMaxMana = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyAggro
    {
        get => _totalPenaltyAggro;
        set
        {
            _totalPenaltyAggro = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyAttackSpeed
    {
        get => _totalPenaltyAttackSpeed;
        set
        {
            _totalPenaltyAttackSpeed = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyCritHitChance
    {
        get => _totalPenaltyCritHitChance;
        set
        {
            _totalPenaltyCritHitChance = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyDamage
    {
        get => _totalPenaltyDamage;
        set
        {
            _totalPenaltyDamage = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyDefense
    {
        get => _totalPenaltyDefense;
        set
        {
            _totalPenaltyDefense = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyKnockback
    {
        get => _totalPenaltyKnockback;
        set
        {
            _totalPenaltyKnockback = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyKnockbackResistance
    {
        get => _totalPenaltyKnockbackResistance;
        set
        {
            _totalPenaltyKnockbackResistance = value;
            Unit.UpdateStats();
        }
    }

    public float TotalPenaltyMoveSpeed
    {
        get => _totalPenaltyMoveSpeed;
        set
        {
            _totalPenaltyMoveSpeed = value;
            Unit.UpdateStats();
        }
    }
    #endregion


    // Powerup Related Code
    public List<PowerupList> powerups = new List<PowerupList>();



    private void OnEnable()
    {
        _updateTime = updateSpeed;
    }

    private void Awake()
    {
        Unit = GetComponent<UnitManager>();
    }

    void Start()
    {
    }

    private void FixedUpdate()
    {
        _powerupUpdateTimer();
    }

    #region Powerup Update

    private void _powerupUpdateTimer()
    {
        if (_updateTime > 0)
        {
            _updateTime -= Time.deltaTime;
        }

        if (_updateTime <= 0)
        {
            _callPowerupUpdate();
            _updateTime = updateSpeed;
        }
    }

    private void _callPowerupUpdate()
    {
        foreach (PowerupList p in powerups)
        {
            p.powerups.Update(this, p.stack);
        }
    }

    IEnumerator ICallPowerupUpdate()
    {
        foreach (PowerupList p in powerups)
        {
            p.powerups.Update(this, p.stack);
        }
        yield return new WaitForSeconds(updateSpeed);
        StartCoroutine(ICallPowerupUpdate());
    }

    public void CallPowerupOnPickup()
    {
        foreach (PowerupList p in powerups)
        {
            p.powerups.OnPowerupPickup(this, p.stack);
        }
    }

    #endregion

    #region Calculate Stats
    // Clear stats
    private void _resetAllBonusesAndPenalties()
    {
        TotalBonusMaxHealth = 0;
        TotalBonusMaxMana = 0;
        TotalBonusAggro = 0;
        TotalBonusAttackSpeed = 0;
        TotalBonusCritHitChance = 0;
        TotalBonusDamage = 0;
        TotalBonusDefense = 0;
        TotalBonusKnockback = 0;
        TotalBonusKnockbackResistance = 0;
        TotalBonusMovementSpeed = 0;

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
    // Add stats

    public void CalculateStatsFromPowerups()
    {
        _resetAllBonusesAndPenalties();
        foreach (PowerupList p in powerups)
        {
            p.powerups.GetStatsFromPowerups(this, p.stack);
        }
    }

    #endregion
}
