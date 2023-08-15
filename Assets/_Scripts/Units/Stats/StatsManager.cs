using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private bool debugMode;

    private UnitManager _unit;

    // DEFAULT STATS
    private float _defaultMaxHealth;
    private float _defaultMaxMana;
    private float _defaultAggro;
    private float _defaultAttackSpeed;
    private float _defaultCritHitChance;
    private float _defaultBaseDamage; // idk about this, change in the future maybe
    private float _defaultDefense;
    private float _defaultHealthRegen; // idk about this too, i just placed it here just in case
    private float _defaultKnockback;
    private float _defaultKnockbackResistance;
    private float _defaultMoveSpeed;

    // BONUS STATS
    private float _bonusMaxHealth;
    private float _bonusMaxMana;
    private float _bonusAggro;
    private float _bonusAttackSpeed;
    private float _bonusCritHitChance;
    private float _bonusDamage;
    private float _bonusDefense;
    private float _bonusHealthRegen;
    private float _bonusKnockback;
    private float _bonusKnockbackResistance;
    private float _bonusCurrentMoveSpeed;

    // PENALTY STATS
    private float _penaltyMaxHealth;
    private float _penaltyMaxMana;
    private float _penaltyAggro;
    private float _penaltyAttackSpeed;
    private float _penaltyCritHitChance;
    private float _penaltyDamage;
    private float _penaltyDefense;
    private float _penaltyHealthRegen;
    private float _penaltyKnockback;
    private float _penaltyKnockbackResistance;
    private float _penaltyCurrentMoveSpeed;

    // TOTAL STATS
    private float _totalMaxHealth;
    private float _totalMaxMana;
    private float _totalAggro;
    private float _totalAttackSpeed;
    private float _totalCritHitChance;
    private float _totalDamage;
    //private float _totalCurrentWeaponDamage;
    private float _totalDefense;
    private float _totalHealthRegen;
    private float _totalKnockback;
    private float _totalKnockbackResistance;
    private float _totalMovementSpeed;


    // DEFAULT STATS GETTERS
    #region DEFAULT STATS
    public float DefaultMaxHealth { get => _defaultMaxHealth; }
    public float DefaultMaxMana { get => _defaultMaxMana; }
    public float DefaultAggro { get => _defaultAggro; }
    public float DefaultAttackSpeed { get => _defaultAttackSpeed; }
    public float DefaultCritHitChance { get => _defaultCritHitChance; }
    public float DefaultBaseDamage { get => _defaultBaseDamage; }
    public float DefaultDefense { get => _defaultDefense; }
    public float DefaultHealthRegen { get => _defaultHealthRegen; }
    public float DefaultKnockback { get => _defaultKnockback; }
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
    public float PenaltyKnockback // TENACITY
    {
        get => _penaltyKnockback;
        set => _penaltyKnockback = value;
    }
    public float PenaltyKnockbackResistance { get => _penaltyKnockbackResistance; set => _penaltyKnockbackResistance = value; }
    public float PenaltyMoveSpeed // SLOWNESS
    {
        get => _penaltyCurrentMoveSpeed;
        set => _penaltyCurrentMoveSpeed = value;
    }
    #endregion

    #region TOTAL STATS
    // TOTAL STATS GETTERS
    public float TotalMaxHealth
    {
        get => _totalMaxHealth;
        set => _totalMaxHealth = value;
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
    //public float TotalCurrentWeaponDamage 
    //{ 
    //    get => _totalCurrentWeaponDamage; 
    //    set => _totalCurrentWeaponDamage = value; 
    //}
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
        StartCoroutine(calculateTotalStats(0.1f));
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

    // MAKE CHARGING SPEED SCALE WITH ATTACK SPEED
    public void CalculateTotalAttackSpeed()
    {
        // If Total Attack Speed goes below 0, it will default to 1
        TotalAttackSpeed = TotalAttackSpeed <= 1 ? 1 : DefaultAttackSpeed + (DefaultAttackSpeed * (BonusAttackSpeed - PenaltyAttackSpeed) * 0.01f);
    }

    #endregion

    #region Critical Hit Chance Calculator
    // must be added after defense is calculated
    // critical hit is always damage * 2
    // default critical hit chance = 4%

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

    public void CalculateCurrentWeaponDamage(float baseDamage)
    {
        //TotalCurrentWeaponDamage = Mathf.Round(baseDamage * (1 + BonusDamage / 100));

    }

    public void CalculateTotalWeaponBaseDamage(float baseDamage)
    {
        TotalDamage = Mathf.Round(baseDamage * (1 + BonusDamage / 100));
    }

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
    // calculate knockback after crit calculation, or LAST
    // calculate knockback resistance

    // knockback cap

    public float CalculateTrueKnockback(float baseWeaponKnockback)
    {
        //TotalKnockback = DefaultKnockback + BonusKnockback;
        // TEMPORARY
        BonusKnockback = baseWeaponKnockback; // Add other factors
        TotalKnockback = DefaultKnockback + BonusKnockback - PenaltyKnockback;
        return TotalKnockback;
    }

    public float CalculateTotalKnockback(float receiverKnockbackResistance)
    {
        // 100 knockback resistance will always half the knockback dealt (may change this)
        float netKb = TotalKnockback / (receiverKnockbackResistance / 100 + 1);
        if(debugMode) Debug.Log("Total Knockback Received: " + TotalKnockback + " Total Knocback Dealt After Calculations: " + netKb);
        return netKb;
    }
    #endregion

    #region Movement Speed Calculator
    private void CalculateTotalMovementSpeed()
    {
        TotalMoveSpeed = DefaultMoveSpeed + BonusMoveSpeed - PenaltyMoveSpeed;
    }
    #endregion

    #region Knockback Resistance Calculator
    private void CalculateTotalKnockbackResistance()
    {
        TotalKnockbackResistance = DefaultKnockbackResistance + BonusKnockbackResistance - PenaltyKnockbackResistance;
    }
    #endregion

    #region Stats Setters

    public void SetDefaultStats(DefaultStatsSO defaultStats)
    {
        if (defaultStats != null)
        {
            _defaultMaxHealth = defaultStats.DefaultMaxHealth;
            _defaultMaxMana = defaultStats.DefaultMaxMana;
            _defaultAggro = defaultStats.DefaultAggro;
            _defaultAttackSpeed = defaultStats.DefaultAttackSpeed;
            _defaultCritHitChance = defaultStats.DefaultCritHitChance;
            _defaultBaseDamage = defaultStats.DefaultBaseDamage; // idk about this, change in the future maybe
            _defaultDefense = defaultStats.DefaultDefense;
            _defaultHealthRegen = defaultStats.DefaultHealthRegen; // idk about this too, i just placed it here just in case
            _defaultKnockback = defaultStats.DefaultKnockback;
            _defaultKnockbackResistance = defaultStats.DefaultKnockbackResistance;
            _defaultMoveSpeed = defaultStats.DefaultMoveSpeed;
        }
    }

    private void _getStatsfromUnit()
    {
        #region listedStats
        //_bonusMaxHealth = _unit.StatList[0];
        //_bonusMaxMana = _unit.StatList[1];
        //_bonusAggro = _unit.StatList[2];
        //_bonusAttackSpeed = _unit.StatList[3];
        //_bonusCritHitChance = _unit.StatList[4];
        //_bonusDamage = _unit.StatList[5];
        //_bonusDefense = _unit.StatList[6];
        //_bonusHealthRegen = _unit.StatList[7];
        //_bonusKnockback = _unit.StatList[8];
        //_bonusKnockbackResistance = _unit.StatList[9];
        //_bonusCurrentMoveSpeed = _unit.StatList[10];
        //_penaltyMaxHealth = _unit.StatList[11];
        //_penaltyMaxMana = _unit.StatList[12];
        //_penaltyAggro = _unit.StatList[13];
        //_penaltyAttackSpeed = _unit.StatList[14];
        //_penaltyCritHitChance = _unit.StatList[15];
        //_penaltyDamage = _unit.StatList[16];
        //_penaltyDefense = _unit.StatList[17];
        //_penaltyHealthRegen = _unit.StatList[18];
        //_penaltyKnockback = _unit.StatList[19];
        //_penaltyKnockbackResistance = _unit.StatList[20];
        //_penaltyCurrentMoveSpeed = _unit.StatList[21]; 
        #endregion

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
        CalculateTotalDefense();
        CalculateTotalCritChance();
        CalculateTotalKnockbackResistance();
        CalculateTotalMovementSpeed();
    }

    private void _calculateAllStats()
    {
        _unit.CalculateBonusPenaltyStats();
        _getStatsfromUnit();
        _calculateTotalStats();
    }

    // TEMPORARY
    IEnumerator calculateTotalStats(float numOfSeconds)
    {
        yield return new WaitForSeconds(numOfSeconds);
        _calculateTotalStats();

        StartCoroutine(calculateTotalStats(numOfSeconds));
    }

    // Reset
    public void SetStatsToDefault(DefaultStatsSO defaultStats)
    {
        //_currentMaxHealth = DefaultMaxHealth;
        //_currentMaxMana = DefaultMaxMana;
        //_currentAggro = DefaultAggro;
        //_currentAttackSpeed = DefaultAttackSpeed;
        //_currentCritHitChance = DefaultCritHitChance;
        //_currentBaseDamage = DefaultBaseDamage; // idk about this, change in the future maybe
        //_currentDefense = DefaultDefense;
        //_currentHealthRegen = DefaultHealthRegen; // idk about this too, i just placed it here just in case
        //_currentKnockback = DefaultKnockback;
        //_currentMoveSpeed = DefaultMoveSpeed;
    }

    #endregion

}
