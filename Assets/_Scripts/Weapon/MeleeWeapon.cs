using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{


    // Weapon Rotation Variables
    [SerializeField] private float totalAtkSpeed;
    [SerializeField] private float weaponAngle;
    private float angleOfTheWeapon = 90;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private Vector3 target;

    // Melee Swing Variables
    private float _swingAngle;
    private float _angle;
    [SerializeField] private float _swing = 1;

    // Melee Thrust Variables
    [SerializeField] private Vector2 targetThrustPosition;
    [SerializeField] private float thrustDistance;
    [SerializeField] private float currentThrustPosition;
    [SerializeField] private float thrustSpeed;
    [SerializeField] private float thrust;

    // Attack Combo Variables
    [SerializeField] private float attackComboGraceTime = 0.3f;
    [SerializeField] private float attackComboTime;
    [SerializeField] private int currentCombo;
    
    // Type of Attack
    [SerializeField] private bool isUsingSwingAttack;
    [SerializeField] private bool isUsingThrustAttack;
    private int _currentWeapon = 1;


    // Weapon Direction Variables
    [SerializeField] private bool isLookingLeft;

    // Weapon Action Variables
    private bool _swinging;
    [SerializeField] private bool _thrusting;



    // On Variable Change Using Swinging
    public bool IsUsingSwingAttack 
    { 
        get => isUsingSwingAttack;
        set 
        { 
            isUsingSwingAttack = value;
            if (isUsingSwingAttack)
            {
                _setSwingingPosition();
            }

            if (!isUsingSwingAttack)
            {
                _setThrustingPosition();
            }
            _swingAngle = 0;           
        }
    }

    private void Awake()
    {
        _anchor = gameObject.transform.gameObject;
        Sprite = GetComponentInChildren<SpriteRenderer>();


        // Input System Variables

        playerControls = new PlayerInputActions();
        Fire = playerControls.Player.Fire;
        ChangeAttack = playerControls.Player.ChangeAttack;
    }

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += Attack;

        ChangeAttack.Enable();
        ChangeAttack.performed += ChangeCurrentAttack;
    }

    private void OnDisable()
    {
        Fire.performed -= Attack;
        Fire.Disable();
        ChangeAttack.performed -= ChangeCurrentAttack;
        ChangeAttack.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _getMousePosition();
        _weaponSpriteFlipper();
        _weaponAttackHandler();
        _rotateWeaponAroundAnchor();
    }

    private void _weaponAttackHandler()
    {

        //if (isUsingSwingAttack)
        //{
        //    _getSwingAngle();
        //    _calculateWeaponSwingTrajectory();
        //} else
        //{
        //    _calculateWeaponThrustTrajectory();
        //}

        _determineAttack();
        _comboTimer();


    }

    #region Weapon Combo Handler
    private void _attackHandler()
    {
        if (_swinging || _thrusting) return;

        if (currentCombo < 3)
        {
            attackComboTime = attackComboGraceTime;
            currentCombo += 1;
        }

        switch (currentCombo)
        {
            case 1:
                _setSwingingPosition();
                _swingWeapon();
                break;
            case 2:
                _swingWeapon();
                break;
            case 3:
                _setThrustingPosition();
                if (_thrusting) return;
                _thrustWeapon();
                break;
        }
    }



    // DETERMINS WHAT ATTACK
    private void _determineAttack()
    {
        switch (currentCombo)
        {
            case 1:
                _doWeaponSwing();
                break;
            case 2:
                _doWeaponSwing();
                break;
            case 3:
                _doWeaponThrust();
                break;
            default:
                _doReturnToDefaultPosition();
                break;
        }
    }

    private void _doReturnToDefaultPosition()
    {
        if (isLookingLeft)
        {
            _swing = -1;
        }

        if (!isLookingLeft)
        {
            _swing = 1;
        }
        _doWeaponSwing();
        _resetCombo();
    }

    private void _doWeaponThrust()
    {
        _calculateWeaponThrustTrajectory();
    }

    private void _doWeaponSwing()
    {
        _getSwingAngle();
        _calculateWeaponSwingTrajectory();
    }

    private void _comboTimer()
    {
        if (attackComboTime > 0 && (!_swinging || !_thrusting))
        {
            attackComboTime -= Time.deltaTime;
        }
        if (attackComboTime <= 0)
        {
            _doReturnToDefaultPosition();
        }
    }

    private void _resetCombo()
    {
        currentCombo = 0;
    }

    #endregion

    #region Weapon Swing Functions

    private void _setSwingingPosition()
    {
        transform.localPosition = new Vector2(0, 0.5f);
        Sprite.transform.localPosition = new Vector2(0.5f, 0.15f);
    }

    private void _swingWeapon()
    {
        if (_swinging) return;

        // Attack
        _swing *= -1;
        _swinging = true;
    }

    private void _calculateWeaponSwingTrajectory()
    {
        // Weapon Swing
        float t = _swing == 1 ? 0 : -225;
        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * totalAtkSpeed);
        if (Mathf.Abs(t - target.z) < 5 && _swinging)
        {
            //_swing *= -1; // Double Swing
            _swinging = false;
        }
        transform.localRotation = Quaternion.Euler(target);

    }

    private void _getSwingAngle()
    {
        _swingAngle = Mathf.Lerp(_swingAngle, _swing * (angleOfTheWeapon), Time.deltaTime * totalAtkSpeed);
    }

    #endregion

    #region Weapon Thrust Functions
    private void _setThrustingPosition()
    {
        _swingAngle = 0;
        transform.localPosition = new Vector2(0, 0.3f);
        Sprite.transform.localPosition = new Vector2(0, 0.3f);
    }

    private void _calculateWeaponThrustTrajectory()
    {
        targetThrustPosition = new Vector2(0, thrustDistance);
        Vector2 defaultThrustPosition = new Vector2(0, 0.3f);
        currentThrustPosition = Mathf.MoveTowards(currentThrustPosition, thrust, thrustSpeed * Time.deltaTime);
        Sprite.gameObject.transform.localPosition = Vector2.Lerp(defaultThrustPosition, targetThrustPosition, currentThrustPosition);
        Vector2 weaponPosition = Sprite.gameObject.transform.localPosition;

        _retractThrustWeapon(weaponPosition);

        if (weaponPosition == defaultThrustPosition)
        {
            _thrusting = false;
            _resetCombo();
        }
    }

    private void _retractThrustWeapon(Vector2 weaponPosition)
    {
        if (weaponPosition == targetThrustPosition && _thrusting)
        {
            thrust = 0;
        }
    }

    private void _thrustWeapon()
    {
        if (_thrusting) return;

        thrust = thrust == 0f ? 1 : 0f;
        _thrusting = true;
    }



    #endregion

    #region Sprite Flipper
    private void _weaponSpriteFlipper()
    {
        // Looking Left
        if (_mousePos.x < 0)
        {
            //_flipSpriteLookingLeft();

            if (!isLookingLeft && !_swinging)
            {
                _swing = -1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = true;
            }


        }


        // Looking Right
        if (_mousePos.x > 0)
        {
            //_flipSpriteLookingRight();

            if (isLookingLeft && !_swinging)
            {
                _swing = 1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = false;
            }

        }

    }



    private void _flipSpriteLookingRight()
    {
        if (_swing == 1 && !_swinging)
        {
            Sprite.flipX = true;
        }
        else if (_swing == -1 && !_swinging)
        {
            Sprite.flipX = false;
        }
    }

    private void _flipSpriteLookingLeft()
    {
        if (_swing == 1 && !_swinging)
        {
            Sprite.flipX = false;
        }
        else if (_swing == -1 && !_swinging)
        {
            Sprite.flipX = true;
        }
    }
    #endregion

    #region Weapon Rotation and Position

    private void _rotateWeaponAroundAnchor()
    {
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle + _swingAngle;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
    }
    #endregion

    private void Attack(InputAction.CallbackContext context)
    {

        _attackHandler();

        //if (isUsingSwingAttack)
        //{
        //    _swingWeapon();
        //} else
        //{
        //    _thrustWeapon();
        //}
    }

    private void ChangeCurrentAttack(InputAction.CallbackContext context)
    {
        if (!_swinging)
        {
            _currentWeapon *= -1;

            switch (_currentWeapon)
            {
                case 1:
                    IsUsingSwingAttack = true;
                    break;
                case -1:
                    IsUsingSwingAttack = false;
                    break;
                default:
                    break;
            }
        }
    }

}
