using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class AstralDeathRaySpell : Spell
{
    [Header("Charging Settings/Variables")]
    [SerializeField] private bool isCharging;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 100;
    [SerializeField] private float chargeSpeed = 1;
    private float defaultCharge = 0;

    [Header("Laser Variables")]
    private AstralDeathRayBehavior _laserPrefab;
    private AstralDeathRayBehavior _laser;
    private ParticleSystem _exhaust;
    [SerializeField] private float laserSize;
    [SerializeField] private float laserDistance = 10;
    [SerializeField] private float laserRotationSpeed = 30;

    [Header("Light Variables")]
    private Light2D light2D;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float defaultLightIntensity = 0;

    [Header("Wand Actions")]
    [SerializeField] private bool canSwing = false;
    [SerializeField] private float wandAngle = 0;
    [SerializeField] private bool canRotate = false;

    public float CurrentCharge
    {
        get => currentCharge;
        set
        {
            if (value > maxCharge)
            {
                currentCharge = maxCharge;
                _activateLaser();
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
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed, LaserSize);
        } 
    
    }
    public float LaserDistance 
    { 
        get => laserDistance;
        set
        {
            laserDistance = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed, LaserSize);
        }
    }
    public float LaserRotationSpeed 
    { 
        get => laserRotationSpeed;
        set
        {
            laserRotationSpeed = value;
            _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed, LaserSize);
        }
    }

    private void SetChargeAmount(float charge, float maxcharge)
    {
        spellCharge.CurrentCharge = charge / maxcharge;
    }

    private void OnEnable()
    {
        _SubscribeFunctionsToCharge();
        _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed, LaserSize);
        
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
        SetSpellVariables();
        SetChargingInputActions(this);
        SetMagicWeaponActions(canSwing, wandAngle, canRotate);
        _spawnLaser();
        _exhaust = GetComponentInChildren<ParticleSystem>();
        light2D = GetComponent<Light2D>();
        
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

    #region Laser Settings/Laser Spawn
    private void _setLaserSettings()
    {
        //_startPoint = transform.position;
        //_endPoint = wand.MouseWorldPosition;
        if (_laser != null && _laser.gameObject.activeSelf) _laser.SetLaserSettings(LaserDistance, LaserRotationSpeed, LaserSize);
    }
    private void _spawnLaser()
    {
        _laserPrefab = Resources.Load<AstralDeathRayBehavior>("Player/Weapons/Magic/Spells/AstralDeathRay/AstralDeathRayPrefab");
        var deathRay = Instantiate(_laserPrefab, Vector3.zero, Quaternion.identity);
        _laser = deathRay;
        _laser._setSpell(this);
    } 
    #endregion

    #region Charge Functions
    private void _chargeHandler()
    {
        if (isCharging)
        {
            CurrentCharge += Time.deltaTime * chargeSpeed;
        }
    }

    private void StartCharge(InputAction.CallbackContext context)
    {
        // Start charging
        isCharging = true;
        spellCharge.EnableSpellCharge();
    }

    private void EndCharge(InputAction.CallbackContext context)
    {
        // Release Charging
        disableLaser();
    } 
    #endregion

    #region Activate/Deactivate Laser Functions
    private void disableLaser()
    {
        isCharging = false;
        CurrentCharge = defaultCharge;
        spellCharge.DisableSpellCharge();
        _exhaust.Play();
    }

    private void _activateLaser()
    {
        _laser?.ActivateLaser(LaserSize);
    }

    private void _deactivateLaser()
    {
        _laser?.DeactivateLaser();
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
