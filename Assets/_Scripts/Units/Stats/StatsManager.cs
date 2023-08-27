using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatusEffectsManager), typeof(PowerupsManager))]
public class StatsManager : MonoBehaviour
{
    [SerializeField] private bool debugMode;

    public bool DebugMode => debugMode;

    private UnitManager _unit;

    #region PRIVATE STATS
    [Header("DEFAULT STATS")]
    [SerializeField] private float _defaultWeaponDamage;
    [SerializeField] private float _defaultWeaponKnockback;
    [SerializeField] private float _defaultWeaponAttackSpeed;


    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMaxMana;
    [SerializeField] private float _defaultAggro;
    [SerializeField] private float _defaultAttackSpeed;
    [SerializeField] private float _defaultCritHitChance;
    [SerializeField] private float _defaultDamage;
    [SerializeField] private float _defaultDefense;
    [SerializeField] private float _defaultHealthRegen; // idk about this too, i just placed it here just in case
    [SerializeField] private float _defaultKnockback;
    [SerializeField] private float _defaultKnockbackResistance;
    [SerializeField] private float _defaultMoveSpeed;

    [Header("BONUS STATS")]
    [SerializeField] private float _bonusMaxHealth;
    [SerializeField] private float _bonusMaxMana;
    [SerializeField] private float _bonusAggro;
    [SerializeField] private float _bonusAttackSpeed;
    [SerializeField] private float _bonusCritHitChance;
    [SerializeField] private float _bonusDamage;
    [SerializeField] private float _bonusDefense;
    [SerializeField] private float _bonusHealthRegen;
    [SerializeField] private float _bonusKnockback;
    [SerializeField] private float _bonusKnockbackResistance;
    [SerializeField] private float _bonusCurrentMoveSpeed;

    [Header("PENALTY STATS")]
    [SerializeField] private float _penaltyMaxHealth;
    [SerializeField] private float _penaltyMaxMana;
    [SerializeField] private float _penaltyAggro;
    [SerializeField] private float _penaltyAttackSpeed;
    [SerializeField] private float _penaltyCritHitChance;
    [SerializeField] private float _penaltyDamage;
    [SerializeField] private float _penaltyDefense;
    [SerializeField] private float _penaltyHealthRegen;
    [SerializeField] private float _penaltyKnockback;
    [SerializeField] private float _penaltyKnockbackResistance;
    [SerializeField] private float _penaltyCurrentMoveSpeed;

    [Header("TOTAL STATS")]
    [SerializeField] private float _totalMaxHealth;
    [SerializeField] private float _totalMaxMana;
    [SerializeField] private float _totalAggro;
    [SerializeField] private float _totalAttackSpeed;
    [SerializeField] private float _totalCritHitChance;
    [SerializeField] private float _totalDamage;
    [SerializeField] private float _totalDefense;
    [SerializeField] private float _totalHealthRegen;
    [SerializeField] private float _totalKnockback;
    [SerializeField] private float _totalKnockbackResistance;
    [SerializeField] private float _totalMovementSpeed;
    #endregion

    #region DEFAULT STATS
    // DEFAULT STATS GETTERS

    public float DefaultWeaponDamage { get => _defaultWeaponDamage; set => _defaultWeaponDamage = value; }
    public float DefaultWeaponKnockback { get => _defaultWeaponKnockback; set => _defaultWeaponKnockback = value; }
    public float DefaultWeaponAttackSpeed { get => _defaultWeaponAttackSpeed; set => _defaultWeaponAttackSpeed = value; }
    public float DefaultMaxHealth { get => _defaultMaxHealth; }
    public float DefaultMaxMana { get => _defaultMaxMana; }
    public float DefaultAggro { get => _defaultAggro; }
    public float DefaultAttackSpeed { get => _defaultAttackSpeed; set => _defaultAttackSpeed = value; }
    public float DefaultCritHitChance { get => _defaultCritHitChance; }
    public float DefaultDamage { get => _defaultDamage; set => _defaultDamage = value; }
    public float DefaultDefense { get => _defaultDefense; }
    public float DefaultHealthRegen { get => _defaultHealthRegen; }
    public float DefaultKnockback { get => _defaultKnockback; set => _defaultKnockback = value; }
    public float DefaultKnockbackResistance { get => _defaultKnockbackResistance; set => _defaultKnockbackResistance = value; }
    public float DefaultMoveSpeed { get => _defaultMoveSpeed; }
    #endregion

    #region BONUS STATS
    // BONUS STATS GETTERS
    public float BonusMaxHealth
    {
        get => _bonusMaxHealth;
        set => _bonusMaxHealth = value;
    }
    public float BonusMaxMana
    {
        get => _bonusMaxMana;
        set => _bonusMaxMana = value;
    }
    public float BonusAggro
    {
        get => _bonusAggro;
        set => _bonusAggro = value;
    }
    public float BonusAttackSpeed
    {
        get => _bonusAttackSpeed;
        set => _bonusAttackSpeed = value;
    }
    public float BonusCritHitChance
    {
        get => _bonusCritHitChance;
        set => _bonusCritHitChance = value;
    }
    public float BonusDamage
    {
        get => _bonusDamage;
        set => _bonusDamage = value;
    }
    public float BonusDefense
    {
        get => _bonusDefense;
        set => _bonusDefense = value;
    }
    public float BonusHealthRegen
    {
        get => _bonusHealthRegen;
        set => _bonusHealthRegen = value;
    }
    public float BonusKnockbackResistance { get => _bonusKnockbackResistance; set => _bonusKnockbackResistance = value; }
    public float BonusKnockback
    {
        get => _bonusKnockback;
        set => _bonusKnockback = value;
    }
    public float BonusMoveSpeed
    {
        get => _bonusCurrentMoveSpeed;
        set => _bonusCurrentMoveSpeed = value;
    }
    #endregion

    #region PENALTY STATS
    // PENALTY STATS GETTERS AND SETTERS
    public float PenaltyMaxHealth
    {
        get => _penaltyMaxHealth;
        set => _penaltyMaxHealth = value;
    }
    public float PenaltyMaxMana
    {
        get => _penaltyMaxMana;
        set => _penaltyMaxMana = value;
    }
    public float PenaltyAggro
    {
        get => _penaltyAggro;
        set => _penaltyAggro = value;
    }
    public float PenaltyAttackSpeed // SLOWNESS
    {
        get => _penaltyAttackSpeed;
        set => _penaltyAttackSpeed = value;
    }
    public float PenaltyCritHitChance
    {
        get => _penaltyCritHitChance;
        set => _penaltyCritHitChance = value;
    }
    public float PenaltyDamage
    {
        get => _penaltyDamage;
        set => _penaltyDamage = value;
    }
    public float PenaltyDefense
    {
        get => _penaltyDefense;
        set => _penaltyDefense = value;
    }
    public float PenaltyHealthRegen
    {
        get => _penaltyHealthRegen;
        set => _penaltyHealthRegen = value;
    }
    public float PenaltyKnockback // Making knockback weaker
    {
        get => _penaltyKnockback;
        set => _penaltyKnockback = value;
    }

    public float PenaltyKnockbackResistance // Making character weaker to knockback
    {
        get => _penaltyKnockbackResistance;
        set => _penaltyKnockbackResistance = value;
    }
    public float PenaltyMoveSpeed // SLOWNESS
    {
        get => _penaltyCurrentMoveSpeed;
        set
        {
            _penaltyCurrentMoveSpeed = value;
        }
    }
    #endregion

    #region TOTAL STATS
    // TOTAL STATS GETTERS
    public float TotalMaxHealth
    {
        get => _totalMaxHealth;
        set
        {
            if (_totalMaxHealth == value) return;
            _totalMaxHealth = value;
            var health = _unit.UnitHealth;
            health?.SetMaxValue(_totalMaxHealth);
        }
    }
    public float TotalMaxMana
    {
        get => _totalMaxMana;
        set => _totalMaxMana = value;
    }

    public float TotalAggro
    {
        get => _totalAggro;
        set => _totalAggro = value;
    }
    public float TotalAttackSpeed
    {
        get => _totalAttackSpeed;
        set => _totalAttackSpeed = value;
    }
    public float TotalCritHitChance
    {
        get => _totalCritHitChance;
        set => _totalCritHitChance = value;
    }
    public float TotalDamage
    {
        get => _totalDamage;
        set => _totalDamage = value;
    }

    public float TotalDefense
    {
        get => _totalDefense;
        set => _totalDefense = value;
    }
    public float TotalHealthRegen
    {
        get => _totalHealthRegen;
        set => _totalHealthRegen = value;
    }
    public float TotalKnockback
    {
        get => _totalKnockback;
        set => _totalKnockback = value;
    }
    public float TotalKnockbackResistance { get => _totalKnockbackResistance; set => _totalKnockbackResistance = value; }
    public float TotalMoveSpeed
    {
        get => _totalMovementSpeed;
        set => _totalMovementSpeed = value;
    }

    #endregion


    private void Awake()
    {
        _unit = GetComponent<UnitManager>();
        _unit.StatUpdate += _calculateAllStats;
    }
    void Start()
    {
        _calculateAllStats();
    }

    #region Max Health Calculator

    public void CalculateTotalMaxHealth()
    {
        TotalMaxHealth = DefaultMaxHealth + BonusMaxHealth - PenaltyMaxHealth;
    }

    #endregion

    #region Max Mana Calculator

    public void CalculateTotalMaxMana()
    {
        TotalMaxMana = DefaultMaxMana + BonusMaxMana - PenaltyMaxMana;
    }

    #endregion

    #region Aggro Calculator
    // The more aggro a player has the more likely it will be targeted by the enemy
    // Will be used for when enemy is targeting the player
    // Will used more when the game is multiplayer


    #endregion

    #region Attack Speed Calculator
    public void CalculateTotalAttackSpeed()
    {
        TotalAttackSpeed = (DefaultAttackSpeed + DefaultWeaponAttackSpeed) * (1 + (BonusAttackSpeed - PenaltyAttackSpeed) / 100);
    }

    #endregion

    #region Critical Hit Chance Calculator
    public void CalculateTotalCritChance()
    {
        TotalCritHitChance = DefaultCritHitChance + BonusCritHitChance;
    }

    public bool isCriticalStrike()
    {
        int critRNG = Random.Range(1, 100);
        if (debugMode)
        {
            Debug.Log("CritRNG :" + critRNG + "/ 100");
            Debug.Log("CritRNG IS:" + (critRNG < TotalCritHitChance ? true : false));
        }
        return critRNG < TotalCritHitChance ? true : false;
    }
    #endregion

    #region Damage Calculator
    public float CalculateTrueDamage(float baseDamage)
    {
        float netDmg = Mathf.Round(baseDamage * (1 + BonusDamage / 100));
        if (debugMode)
        {
            Debug.Log("base damage is: " + baseDamage);
            Debug.Log("total damage is: " + TotalDamage);
        }
        return netDmg;
    }

    public float CalculateFinalDamage(float damage, bool isCrit)
    {
        float netDmg = CalculateDamageReceivedByEnemyWithDefense(CalculateTrueDamage(damage));
        netDmg = Mathf.Round(Random.Range(-netDmg * 0.15f, netDmg * 0.15f) + netDmg);
        netDmg = netDmg <= 0 ? 1 : netDmg;
        if (debugMode)
        {
            Debug.Log("FINAL DAMAGE: " + (isCrit ? netDmg * 2 : netDmg));
            if (isCrit) Debug.Log("CRIT!");
        }
        return isCrit ? netDmg * 2 : netDmg;
    }

    #endregion

    #region Defense Calculator
    public void CalculateTotalDefense()
    {
        TotalDefense = DefaultDefense + BonusDefense - PenaltyDefense;
    }

    public float CalculateDamageReceivedByPlayerWithDefense(float atkDamage)
    {
        // var netDmg = (atkDamage - TotalDefense * factor); // ONLY IF I AM ADDING DIFFICULTY
        float netDmg = (atkDamage - TotalDefense * 0.5f); // 0.5f is the easiest 
        return netDmg < 0 ? 1 : Mathf.Round(netDmg);
    }

    public float CalculateDamageReceivedByEnemyWithDefense(float atkDamage)
    {
        float netDmg = (atkDamage - TotalDefense * 0.5f);
        if (debugMode) Debug.Log(gameObject.name + " has received " + (netDmg < 0 ? 1 : netDmg));
        return netDmg < 0 ? 1 : Mathf.Round(netDmg);
    }

    #endregion

    #region Health Regen Calculator
    // idk if I am going to implement health regen normally
    #endregion

    #region Knockback Calculator
    public float CalculateTrueKnockback()
    {
        TotalKnockback = (DefaultKnockback + DefaultWeaponKnockback) * (1 + (BonusKnockback - PenaltyKnockback) / 100);
        return TotalKnockback;
    }

    public float CalculateTotalKnockback(float receiverKnockbackResistance)
    {
        float netKb = TotalKnockback - (TotalKnockback * (receiverKnockbackResistance / TotalKnockback));
        if (debugMode) Debug.Log("Total Knockback Received: " + TotalKnockback + " Total Knocback Dealt After Calculations: " + netKb);
        return netKb < 0 ? 0 : netKb;
    }
    #endregion

    #region Movement Speed Calculator
    private void CalculateTotalMovementSpeed()
    {
        TotalMoveSpeed = TotalMoveSpeed < 0 ? 0.1f : DefaultMoveSpeed * (1 + (BonusMoveSpeed - PenaltyMoveSpeed) / 100);
    }
    #endregion

    #region Knockback Resistance Calculator
    private void CalculateTotalKnockbackResistance()
    {
        TotalKnockbackResistance = TotalKnockbackResistance < 0 ? 0 : DefaultKnockbackResistance + (BonusKnockbackResistance - PenaltyKnockbackResistance);
    }
    #endregion

    #region Stats Calculators

    public void SetDefaultStats(DefaultStatsSO defaultStats)
    {
        if (defaultStats != null)
        {
            _defaultMaxHealth = defaultStats.DefaultMaxHealth;
            _defaultMaxMana = defaultStats.DefaultMaxMana;
            _defaultAggro = defaultStats.DefaultAggro;
            _defaultAttackSpeed = defaultStats.DefaultAttackSpeed;
            _defaultCritHitChance = defaultStats.DefaultCritHitChance;
            _defaultDamage = defaultStats.DefaultBaseDamage; // For enemies
            _defaultDefense = defaultStats.DefaultDefense;
            _defaultHealthRegen = defaultStats.DefaultHealthRegen;
            _defaultKnockback = defaultStats.DefaultKnockback;
            _defaultKnockbackResistance = defaultStats.DefaultKnockbackResistance;
            _defaultMoveSpeed = defaultStats.DefaultMoveSpeed;
        }
    }

    private void _getStatsfromUnit()
    {
        DefaultWeaponDamage = _unit.UnitWeaponDefaultDamage;
        DefaultWeaponKnockback = _unit.UnitWeaponDefaultKnockback;
        DefaultWeaponAttackSpeed = _unit.UnitWeaponDefaultAttackSpeed;

        // BONUS STATS
        BonusMaxHealth = _unit.TotalBonusMaxHealth;
        BonusMaxMana = _unit.TotalBonusMaxMana;
        BonusAggro = _unit.TotalBonusAggro;
        BonusAttackSpeed = _unit.TotalBonusAttackSpeed;
        BonusCritHitChance = _unit.TotalBonusCritHitChance;
        BonusDamage = _unit.TotalBonusDamage;
        BonusDefense = _unit.TotalBonusDefense;
        BonusHealthRegen = _unit.TotalBonusHealthRegen;
        BonusKnockback = _unit.TotalBonusKnockback;
        BonusKnockbackResistance = _unit.TotalBonusKnockbackResistance;
        BonusMoveSpeed = _unit.TotalBonusMoveSpeed;

        // PENALTY STATS
        PenaltyMaxHealth = _unit.TotalPenaltyMaxHealth;
        PenaltyMaxMana = _unit.TotalPenaltyMaxMana;
        PenaltyAggro = _unit.TotalPenaltyAggro;
        PenaltyAttackSpeed = _unit.TotalPenaltyAttackSpeed;
        PenaltyCritHitChance = _unit.TotalPenaltyCritHitChance;
        PenaltyDamage = _unit.TotalPenaltyDamage;
        PenaltyDefense = _unit.TotalPenaltyDefense;
        PenaltyHealthRegen = _unit.TotalPenaltyHealthRegen;
        PenaltyKnockback = _unit.TotalPenaltyKnockback;
        PenaltyKnockbackResistance = _unit.TotalPenaltyKnockbackResistance;
        PenaltyMoveSpeed = _unit.TotalPenaltyMoveSpeed;
    }

    private void _calculateTotalStats()
    {
        CalculateTotalMaxHealth();
        CalculateTotalMaxMana();
        CalculateTotalAttackSpeed();
        TotalDamage = CalculateTrueDamage(DefaultDamage + DefaultWeaponDamage);
        CalculateTotalDefense();
        CalculateTotalCritChance();
        TotalKnockback = CalculateTrueKnockback();
        CalculateTotalKnockbackResistance();
        CalculateTotalMovementSpeed();
    }

    private void _calculateAllStats()
    {
        _unit.CalculateExtraStats();
        _getStatsfromUnit();
        _calculateTotalStats();
    }

    #endregion

}
