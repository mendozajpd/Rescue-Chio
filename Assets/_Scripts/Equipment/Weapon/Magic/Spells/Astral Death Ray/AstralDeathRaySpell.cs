using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class AstralDeathRaySpell : Spell
{
    [Header("Spell Settings")]
    [SerializeField] private float defaultSpellDamage = 0.5f;
    [SerializeField] private float defaultSpellKnockback;
    [SerializeField] private float defaultSpellCastSpeed;
    [SerializeField] private float defaultManaCost;
    [SerializeField] private bool inflictsKnockback;

    [Header("Charging Settings/Variables")]
    [SerializeField] private bool isCharging;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 100;
    [SerializeField] private float chargeSpeed = 1;
    private float defaultCharge = 0;

    [Header("Laser Variables")]
    private bool _laserActivated;
    private AstralDeathRayBehavior _laserPrefab;
    private AstralDeathRayBehavior _laser;
    private ParticleSystem _exhaust;
    [SerializeField] private float laserSize;
    [SerializeField] private float laserDistance = 10;
    [SerializeField] private float laserRotationSpeed = 30;
    [SerializeField] private float movementspeedReduction;

    [Header("Light Variables")]
    private Light2D light2D;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float defaultLightIntensity = 0;

    [Header("Wand Actions")]
    [SerializeField] private bool canSwing = false;
    [SerializeField] private float wandAngle = 0;
    [SerializeField] private bool canRotate = false;


    // Player
    private StatsManager _unitStats;
    private Mana _unitMana;

    public float CurrentCharge
    {
        get => currentCharge;
        set
        {
            if (value > maxCharge)
            {
                currentCharge = maxCharge;
                if (!_laserActivated) _activateLaser();
            }
            else if (value < maxCharge)
            {
                currentCharge = value;
                _deactivateLaser();
            }

            SetChargeAmount(currentCharge, maxCharge);
        }

    }

    public float LaserSize
    {
        get => laserSize;
        set
        {
            laserSize = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);
        }

    }
    public float LaserDistance
    {
        get => laserDistance;
        set
        {
            laserDistance = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);
        }
    }
    public float LaserRotationSpeed
    {
        get => laserRotationSpeed;
        set
        {
            laserRotationSpeed = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);
        }
    }

    public bool InflictsKnockback
    {
        get => inflictsKnockback;
        set
        {
            inflictsKnockback = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);

        }
    }

    private void SetChargeAmount(float charge, float maxcharge)
    {
        spellCharge.CurrentCharge = charge / maxcharge;
    }

    private void OnEnable()
    {
        _SubscribeFunctionsToCharge();
        _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);

    }

    private void OnDisable()
    {
        _UnsubscribeFunctionsToCharge();
        disableLaser();
    }

    private void OnDestroy()
    {
        DisableChargingInputActions(this);
        Destroy(_laser.gameObject);
    }

    #region SubscribeFunctions
    private void _SubscribeFunctionsToCharge()
    {
        Charging.started += StartCharge;
        Charging.canceled += EndCharge;
    }

    private void _UnsubscribeFunctionsToCharge()
    {
        Charging.started -= StartCharge;
        Charging.canceled -= EndCharge;
    }
    #endregion

    private void Awake()
    {
        SetSpellVariables(defaultSpellDamage, defaultSpellKnockback, defaultSpellCastSpeed, defaultManaCost);
        SetChargingInputActions(this);
        SetMagicWeaponActions(canSwing, wandAngle, canRotate);
        _spawnLaser();
        _exhaust = GetComponentInChildren<ParticleSystem>();
        light2D = GetComponent<Light2D>();
        _unitMana = wand.equipment.Unit.UnitMana;
        _unitStats = wand.equipment.playerStats;

    }



    void Start()
    {
        _setLaserSettings();
    }

    void Update()
    {
        _chargeHandler();
        _lightTimeHandler();
    }

    private void FixedUpdate()
    {
        if (_laserActivated) _unitMana.ConsumeMana(SpellManaCost);
    }

    #region Laser Settings/Laser Spawn
    private void _setLaserSettings()
    {
        if (_laser != null && _laser.gameObject.activeSelf) _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed);
    }
    private void _spawnLaser()
    {
        _laserPrefab = Resources.Load<AstralDeathRayBehavior>("Units/Player/Weapons/Magic/Spells/AstralDeathRay/AstralDeathRayPrefab");
        Transform poolLocation = GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<ProjectilesPool>().transform;
        var deathRay = Instantiate(_laserPrefab, poolLocation);
        _laser = deathRay;
        _laser._setSpell(this);
    }

    #endregion

    #region Charge Functions
    private void _chargeHandler()
    {
        if (isCharging)
        {
            if(_unitMana.CurrentValue < spellManaCost)
            {
                disableLaser();
                return;
            }
            float atkSpeed = _unitStats.TotalAttackSpeed;
            _slowDownPlayer(movementspeedReduction);
            CurrentCharge += Time.deltaTime * (chargeSpeed + (atkSpeed * 0.1f));
        }
    }

    private void _slowDownPlayer(float speedReduction)
    {
        float movementSpeed = speedReduction * (CurrentCharge / maxCharge);
        wand.TotalPenaltyMoveSpeed = movementSpeed;
    }

    private void StartCharge(InputAction.CallbackContext context)
    {
        // Start charging
        isCharging = true;
        spellCharge.EnableSpellCharge();
    }

    private void EndCharge(InputAction.CallbackContext context)
    {
        disableLaser();
    }
    #endregion

    #region Activate/Deactivate Laser Functions
    private void disableLaser()
    {
        if (_laserActivated) _exhaust.Play();
        isCharging = false;
        CurrentCharge = defaultCharge;
        spellCharge.DisableSpellCharge();
        _slowDownPlayer(0);
    }

    private void _activateLaser()
    {
        _laser?.ActivateLaser(LaserSize);
        _laserActivated = true;
    }

    private void _deactivateLaser()
    {
        _laser?.DeactivateLaser();
        _laserActivated = false;
    }
    #endregion

    #region Light Handlers

    private void _lightTimeHandler()
    {
        // Turn on light when charging from 0 - 100%
        if (isCharging)
        {
            light2D.intensity = (currentCharge / maxCharge) * maxLightIntensity;
        }

        if (!isCharging && light2D.intensity > 0)
        {
            light2D.intensity -= Time.deltaTime * (maxLightIntensity * 2);
        }

        if (light2D.intensity < 0)
        {
            light2D.intensity = defaultLightIntensity;
        }

        // when laser is not active, light fades out
    }

    #endregion
}
