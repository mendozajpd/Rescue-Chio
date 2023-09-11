using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StatusEffectsManager : MonoBehaviour
{
    private UnitManager _unit;
    private Health _unitHealth;
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


    public float TotalBonusMaxHealth
    {
        get => _totalBonusMaxHealth;
        set
        {
            _totalBonusMaxHealth = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusMaxMana
    {
        get => _totalBonusMaxMana;
        set
        {
            _totalBonusMaxMana = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusAggro
    {
        get => _totalBonusAggro;
        set
        {
            _totalBonusAggro = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusAttackSpeed
    {
        get => _totalBonusAttackSpeed;
        set
        {
            _totalBonusAttackSpeed = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusCritHitChance
    {
        get => _totalBonusCritHitChance;
        set
        {
            _totalBonusCritHitChance = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusDamage
    {
        get => _totalBonusDamage;
        set
        {
            _totalBonusDamage = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusDefense
    {
        get => _totalBonusDefense;
        set
        {
            _totalBonusDefense = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusKnockback
    {
        get => _totalBonusKnockback;
        set
        {
            _totalBonusKnockback = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusKnockbackResistance
    {
        get => _totalBonusKnockbackResistance;
        set
        {
            _totalBonusKnockbackResistance = value;
            _unit.UpdateStats();
        }
    }

    public float TotalBonusMovementSpeed
    {
        get => _totalBonusMovementSpeed;
        set
        {
            _totalBonusMovementSpeed = value;
            _unit.UpdateStats();
        }
    }

    // PENALTY
    public float TotalPenaltyMaxHealth
    {
        get => _totalPenaltyMaxHealth;
        set
        {
            _totalPenaltyMaxHealth = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyMaxMana
    {
        get => _totalPenaltyMaxMana;
        set
        {
            _totalPenaltyMaxMana = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyAggro
    {
        get => _totalPenaltyAggro;
        set
        {
            _totalPenaltyAggro = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyAttackSpeed
    {
        get => _totalPenaltyAttackSpeed;
        set
        {
            _totalPenaltyAttackSpeed = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyCritHitChance
    {
        get => _totalPenaltyCritHitChance;
        set
        {
            _totalPenaltyCritHitChance = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyDamage
    {
        get => _totalPenaltyDamage;
        set
        {
            _totalPenaltyDamage = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyDefense
    {
        get => _totalPenaltyDefense;
        set
        {
            _totalPenaltyDefense = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyKnockback
    {
        get => _totalPenaltyKnockback;
        set
        {
            _totalPenaltyKnockback = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyKnockbackResistance
    {
        get => _totalPenaltyKnockbackResistance;
        set
        {
            _totalPenaltyKnockbackResistance = value;
            _unit.UpdateStats();
        }
    }

    public float TotalPenaltyMoveSpeed
    {
        get => _totalPenaltyMoveSpeed;
        set
        {
            _totalPenaltyMoveSpeed = value;
            _unit.UpdateStats();
        }
    }


    #endregion

    #region Burning Status Effect
    private float _burningStatusTime;
    public float BurningStatusTime
    {
        get => _burningStatusTime;
        set
        {
            _burningStatusTime = value;
            
        }
    }

    private int _burningStatusTier;
    private float _burningDamageDelayDuration = .4f;
    private float _burningDamageDelayTime;
    private Color32 _burningDamagePopupColor = new Color32(214,133,102,0);
    private ParticleSystem firePrefab;
    private ParticleSystem fireObject;
    private Light2D fireLights;
    private ParticleSystem.EmissionModule fireEmission;
    private float fireIntensity = 1;
    private float fireAmount = 10;
    #endregion

    private void Awake()
    {
        _unit = GetComponent<UnitManager>();

        // Burning Variables
        SpawnFireParticles();
    }



    void Start()
    {
        _unitHealth = _unit.UnitHealth;

    }

    void Update()
    {

    }
    private void FixedUpdate()
    {
        _burningStatusHandler();

    }
    #region Burning Status Effect
    private void _burningStatusHandler()
    {
        _burningStatusTimer();

        if(BurningStatusTime > 0)
        {
            if (!fireLights.enabled) fireLights.enabled = true;
            if (!fireEmission.enabled) fireEmission.enabled = true;

            if (_burningDamageDelayTime < 0 )
            {
                burnUnit();
                fireLights.intensity += Random.Range(.1f, .2f);
                fireLights.intensity -= Random.Range(.1f, .3f);
                fireAmount -= 1;
                fireEmission.rateOverTime = fireAmount;
            }
        }
    }

    private void burnUnit()
    {
        _unitHealth.Damage(1 * _burningStatusTier, false, 0, null, _burningDamagePopupColor);
        _burningDamageDelayTime = _burningDamageDelayDuration;
        fireLights.intensity = fireIntensity * _burningStatusTier;
        fireAmount = 10 * _burningStatusTier;
        fireEmission.rateOverTime = fireAmount;

    }
    private void _burningStatusTimer()
    {
        if (BurningStatusTime > 0)
        {
            BurningStatusTime -= Time.deltaTime;
            _burningDamageDelayTime -= Time.deltaTime;
        }

        if (BurningStatusTime <= 0)
        {
            if (fireEmission.enabled) fireEmission.enabled = false;
            if (fireLights.enabled) fireLights.enabled = false;
        }
    }
    private void SpawnFireParticles()
    {
        firePrefab = Resources.Load<ParticleSystem>("Fire");
        fireObject = Instantiate(firePrefab, transform);
        fireEmission = fireObject.emission;
        fireLights = fireObject.GetComponent<Light2D>();
    }

    public void InflictBurningStatus(float duration, int tier)
    {
        BurningStatusTime = duration;
        _burningStatusTier = tier;
    }
    #endregion

}
