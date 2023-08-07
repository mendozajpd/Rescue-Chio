using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    // DEFAULT STATS
    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMaxMana;
    [SerializeField] private float _defaultAggro;
    [SerializeField] private float _defaultAttackSpeed;
    [SerializeField] private float _defaultCritHitChance;
    [SerializeField] private float _defaultBaseDamage; // idk about this, change in the future maybe
    [SerializeField] private float _defaultDefense;
    [SerializeField] private float _defaultHealthRegen; // idk about this too, i just placed it here just in case
    [SerializeField] private float _defaultKnockback;
    [SerializeField] private float _defaultMoveSpeed;

    // BONUS STATS
    private float _bonusMaxHealth;
    private float _bonusMaxMana;
    private float _bonusAggro;
    private float _bonusAttackSpeed;
    private float _bonusCritHitChance;
    private float _bonusBaseDamage;
    private float _bonusDefense;
    private float _bonusHealthRegen;
    private float _bonusKnockback;
    private float _bonusCurrentMoveSpeed;

    private float _totalMaxHealth;
    private float _totalMaxMana;
    private float _totalAggro;
    private float _totalAttackSpeed;
    private float _totalCritHitChance;
    private float _totalDamage;
    private float _totalDefense;
    private float _totalHealthRegen;
    private float _totalKnockback;
    private float _totalMovementSpeed;


    // DEFAULT STATS GETTERS
    public float DefaultMaxHealth { get => _defaultMaxHealth; }
    public float DefaultMaxMana { get => _defaultMaxMana; }
    public float DefaultAggro { get => _defaultAggro;  }
    public float DefaultAttackSpeed { get => _defaultAttackSpeed; }
    public float DefaultCritHitChance { get => _defaultCritHitChance; }
    public float DefaultBaseDamage { get => _defaultBaseDamage; }
    public float DefaultDefense { get => _defaultDefense;  }
    public float DefaultHealthRegen { get => _defaultHealthRegen; }
    public float DefaultKnockback { get => _defaultKnockback; }
    public float DefaultMoveSpeed { get => _defaultMoveSpeed; }

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
        get => _bonusBaseDamage; 
        set => _bonusBaseDamage = value; 
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
    public float BonusKnockback 
    { 
        get => _bonusKnockback; 
        set => _bonusKnockback = value; 
    }
    public float BonusCurrentMoveSpeed 
    { 
        get => _bonusCurrentMoveSpeed; 
        set => _bonusCurrentMoveSpeed = value; 
    }


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
    public float TotalMovementSpeed 
    { 
        get => _totalMovementSpeed; 
        set => _totalMovementSpeed = value; 
    }


    //[Header("Attack Speed Variables")]

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateStats()
    {

    }

    #region Aggro Calculator
    // The more aggro a player has the more likely it will be targeted by the enemy
    // Will be used for when enemy is targeting the player
    // Will used more when the game is multiplayer


    #endregion

    #region Attack Speed Calculator
    // totalAttackSpeed = defaultAttackspeed + totalBonusattackspeed
    // totalBonusattackspeed = defaultAttackSpeed * bonusAttackSpeed * 0.01
    public void CalculateTotalAttackSpeed()
    {
        TotalAttackSpeed = DefaultAttackSpeed + (DefaultAttackSpeed * BonusAttackSpeed * 0.01f);
    }

    #endregion

    #region Critical Hit Chance Calculator
    // must be added after defense is calculated
    // critical hit is always damage * 2
    // default critical hit chance = 4%



    #endregion

    #region Damage Calculator
    // Get Base Damage
    // Calculate Total Damage
    // Give back total damage to weapon
    // Weapon will calculate how much damage it will output
    //private float GetBaseDamage(Weapon weapon)
    //{
    //    return weapon.BaseDamage;
    //}

    // TOTAL DAMAGE OF THE WEAPON USING RN (BASICALLY TRUE DAMAGE)

    public float CalculateTotalDamage(float baseDamage) 
    {
        Debug.Log("base damage is: " + baseDamage);
        TotalDamage = Mathf.Round(baseDamage * (1 + BonusDamage / 100));
        Debug.Log("total damage is: " + TotalDamage);
        return TotalDamage;
    }
    // must be calculated first
    // Damage will be randomized between the -+damageDifference%

    // get base damage
    // add weapon modifier (i will probably not implement this)
    // add damage bonuses to base damage
    // totalDamage = base damage + (base damage + totalBonusDamage)
    // totalBonusDamage = baseDamage * bonusDamage * 0.01
    // Round down 

    // CREATE A DIFFERENT TOTAL DAMAGE CALCULATOR FOR ENEMY

    #endregion

    #region Defense Calculator
    // THIS IS TO BE CALCULATED WHEN UNIT IS TAKING DAMAGE
    // must be calculated 2nd after damage and before adding critical hit
    // player defense is different to enemy/object defense

    // for players
    // net dmg = (attack damage - def * factor) factor is dependent on difficulty, the higher the factor the more damage player takes
    public void CalculateTotalDefense()
    {
        TotalDefense = DefaultDefense + BonusDefense;
    }

    public float CalculateDamageReceivedByPlayerWithDefense(float atkDamage)
    {
        // var netDmg = (atkDamage - TotalDefense * factor); // ONLY IF I AM ADDING DIFFICULTY
        float netDmg = (atkDamage - TotalDefense * 0.5f); // 0.5f is the easiest 
        return Mathf.Round(netDmg);
    }

    // for enemies
    public float CalculateDamageReceivedByEnemyWithDefense(float atkDamage)
    {
        float netDmg = (atkDamage - TotalDefense * 0.5f);
        Debug.Log(gameObject.name + " has received " + netDmg);
        return Mathf.Round(netDmg);
    }

    #endregion

    #region Health Regen Calculator
    // idk if I am going to implement health regen normally
    #endregion

    #region Knockback Calculator
    // calculate knockback after crit calculation, or LAST
    // calculate knockback resistance

    // knockback cap

    // add extra knockback if it is critical hit
    #endregion

    #region Movement Speed Calculator
    // totalMovespeed = defaultMovespeed + bonusMovespeed - penaltyMovespeed
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
            _defaultMoveSpeed = defaultStats.DefaultMoveSpeed;
        }
    }

    public void CalculateTotalStats()
    {
        CalculateTotalAttackSpeed();
        //CalculateTotalDamage(); // currently cannot be called without a parameter
        CalculateTotalDefense();
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
