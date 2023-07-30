using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AstralDeathRaySpell : Spell
{
    [Header("Charging Settings/Variables")]
    [SerializeField] private bool isCharging;
    [SerializeField] private bool activateLaser;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 100;
    [SerializeField] private float chargeSpeed = 1;
    private float defaultCharge = 0;

    [Header("Laser Variables")]
    [SerializeField] private float laserSize;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

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
                activateLaser = true; // change to void
            }
            else if (value < maxCharge)
            {
                currentCharge = value;
                activateLaser = false;
            }

            SetChargeAmount(currentCharge, maxCharge);
        }

    }

    private void SetChargeAmount(float charge, float maxcharge)
    {
        spellCharge.CurrentCharge = charge / maxcharge;
    }

    private void OnEnable()
    {
        _SubscribeFunctionsToCharge();
    }

    private void OnDisable()
    {
        _UnsubscribeFunctionsToCharge();
    }

    private void OnDestroy()
    {
        DisableChargingInputActions(this);
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
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(isCharging)
        {
            CurrentCharge += Time.deltaTime * chargeSpeed ;
        }

    }

    private void StartCharge(InputAction.CallbackContext context)
    {
        // Start charging
        isCharging = true;
    }

    private void EndCharge(InputAction.CallbackContext context)
    {
        // Release Charging
        isCharging = false;
        CurrentCharge = defaultCharge;
    }





}
