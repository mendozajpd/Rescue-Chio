using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(StatsManager), typeof(StatusEffectsManager))]
public class PlayerManager : UnitManager
{
    [Header("Default Stats")]
    private float _defaultMaxHealth;
    private float _defaultMaxMana;
    private float _defaultAggro;
    private float _defaultAttackSpeed;
    private float _defaultCritHitChance;
    private float _defaultBaseDamage; // idk about this, change in the future maybe
    private float _defaultDefense;
    private float _defaultHealthRegen; // idk about this too, i just placed it here just in case
    private float _defaultKnockback;
    private float _defaultMoveSpeed;

    // Getters
    public float DefaultMaxHealth => _defaultMaxHealth;
    public float DefaultMaxMana => _defaultMaxMana;
    public float DefaultAggro => _defaultAggro;
    public float DefaultAttackSpeed => _defaultAttackSpeed;
    public float DefaultHitChance => _defaultCritHitChance;
    public float DefaultBaseDamage => _defaultBaseDamage; // idk about this, change in the future maybe
    public float DefaultDefense => _defaultDefense;
    public float DefaultHealthRegen => _defaultHealthRegen; // idk about this too, i just placed it here just in case
    public float DefaultKnockback => _defaultKnockback;
    public float DefaultMoveSpeed => _defaultMoveSpeed;

    [Header("Current Stats")]
    public float CurrentMaxHealth;
    public float CurrentMaxMana;
    public float CurrentAggro;
    public float CurrentAttackSpeed;
    public float CurrentHitChance;
    public float CurrentBaseDamage; // idk about this, change in the future maybe
    public float CurrentDefense;
    public float CurrentHealthRegen; // idk about this too, i just placed it here just in case
    public float CurrentKnockback;
    public float CurrentMoveSpeed;

    [Header("Bonus Stats")]
    private float _bonusMaxHealth;
    private float _bonusMaxMana;
    private float _bonusAggro; // kind of ironic
    private float _bonusAttackSpeed;
    private float _bonusHitChance;
    private float _bonusBaseDamage;
    private float _bonusDefense;
    private float _bonusHealthRegen;
    private float _bonusKnockback;
    private float _bonusCurrentMoveSpeed;

    // Bonus Stats Getter - WHENEVER ANY OF THE VALUE CHANGES MAKE IT RECALCULATE STATS
    public float BonusMaxHealth { get => _bonusMaxHealth; set => _bonusMaxHealth = value; }
    public float BonusMaxMana { get => _bonusMaxMana; set => _bonusMaxMana = value; }
    public float BonusAggro { get => _bonusAggro; set => _bonusAggro = value; }
    public float BonusAttackSpeed { get => _bonusAttackSpeed; set => _bonusAttackSpeed = value; }
    public float BonusHitChance { get => _bonusHitChance; set => _bonusHitChance = value; }
    public float BonusBaseDamage { get => _bonusBaseDamage; set => _bonusBaseDamage = value; }
    public float BonusDefense { get => _bonusDefense; set => _bonusDefense = value; }
    public float BonusHealthRegen { get => _bonusHealthRegen; set => _bonusHealthRegen = value; }
    public float BonusKnockback { get => _bonusKnockback; set => _bonusKnockback = value; }
    public float BonusCurrentMoveSpeed { get => _bonusCurrentMoveSpeed; set => _bonusCurrentMoveSpeed = value; }


    private void Awake()
    {
        _setDefaultStatsFromScriptableObject(Resources.Load<DefaultStatsSO>("Player/PlayerDefaultStats"));
        SpawnRequiredComponents(UsesMana);
        UnitHealth.SetMaxHealth(100);
    }

    private void _setDefaultStatsFromScriptableObject(DefaultStatsSO defaultStats)
    {
        if (defaultStats != null)
        {
            _defaultMaxHealth = defaultStats.DefaultMaxHealth;
            UsesMana = defaultStats.UsesMana;
            if(UsesMana) _defaultMaxMana = defaultStats.DefaultMaxMana;
            _defaultAggro = defaultStats.DefaultAggro;
            _defaultAttackSpeed = defaultStats.DefaultAttackSpeed;
            _defaultCritHitChance = defaultStats.DefaultCritHitChance;
            _defaultBaseDamage = defaultStats.DefaultBaseDamage; // idk about this, change in the future maybe
            _defaultDefense = defaultStats.DefaultDefense;
            _defaultHealthRegen = defaultStats.DefaultHealthRegen; // idk about this too, i just placed it here just in case
            _defaultKnockback = defaultStats.DefaultKnockback;
            _defaultMoveSpeed = defaultStats.DefaultMoveSpeed;

            Debug.Log(gameObject.name + " default stats has been set! --- Unit Name: " + defaultStats.UnitName);

        }
        else
        {
            Debug.Log(gameObject.name + " Default Stats are missing!");
        }
    }


}
