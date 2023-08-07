using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{
    // Player Variables
    private Rigidbody2D _rb;

    // Weapon Rotation Variables
    [SerializeField] private float totalAtkSpeed;
    [SerializeField] private float weaponAngle;
    private float angleOfTheWeapon = 90;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private Vector3 target;
    private float _angle;

    // Melee Swing Variables
    private float _swingAngle;
    private float _swing = 1;

    // Melee Thrust Variables
    [SerializeField] private float thrustDistance;
    [SerializeField] private float thrustSpeed;
    private Vector2 _targetThrustPosition;
    private float _currentThrustPosition;
    private float _thrust;

    // Attack Combo Variables
    [SerializeField] private float attackComboGraceTime;
    [SerializeField] private int currentCombo;
    [SerializeField] private float _attackComboTime;
    
    // Weapon Direction Variables
    [SerializeField] private bool isLookingLeft;

    // Weapon Action Variables
    [SerializeField] private bool _swinging;
    [SerializeField] private bool _thrusting;

    // Audio Variables
    public AudioClip[] WeaponSwingAudio;
    private AudioSource audioSource;
    [Range(0.1f, 0.5f)]
    public float volumeChangeMultiplier;
    [Range(0.1f, 0.5f)]
    public float pitchChangeMultiplier;

    public bool Swinging 
    { 
        get => _swinging; 
         
    }
    public bool Thrusting 
    { 
        get => _thrusting; 
    }



    private void Awake()
    {
        // Player Variables
        _rb = GetComponentInParent<PlayerEquipment>().player.GetComponent<Rigidbody2D>();

        // Weapon Variables
        _anchor = gameObject.transform.gameObject;
        Sprite = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Input System Variables
        SetInputVariables();
    }

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += Attack;

    }

    private void OnDisable()
    {
        Fire.performed -= Attack;
        Fire.Disable();
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
        _determineAttack();
        _comboTimer();
    }

    #region Weapon Combo Handler

    // THIS GOES TO INTO THE INPUT SYSTEM ATTACK 
    private void _attackComboHandler()
    {
        if (_swinging || _thrusting) return;

        if (currentCombo < 3)
        {
            _attackComboTime = attackComboGraceTime;
            currentCombo += 1;
        }

        switch (currentCombo)
        {
            case 1:
                _swingAngle = angleOfTheWeapon * _swing;
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



    // DETERMINES WHAT ATTACK DEPENDING ON THE CURRENT COMBO 
    // ALSO BE PLACED IN UPDATE METHOD
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
        _resetCombo();
        _returnToSwingAngle();
        if (isLookingLeft)
        {
            _swing = -1;
        }

        if (!isLookingLeft)
        {
            _swing = 1;
        }
    }

    private void _returnToSwingAngle()
    {
        _setSwingingPosition();
        _getSwingAngle();
        _calculateWeaponSwingTrajectory();
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
        if (_attackComboTime > 0)
        {
            if (!_swinging && !_thrusting)
            {
                _attackComboTime -= Time.deltaTime;
            }
        }
        if (_attackComboTime <= 0)
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
        playRandomMeleeSwing();
        _swinging = true;
    }

    private void _calculateWeaponSwingTrajectory()
    {
        // Weapon Swing
        float t = _swing == 1 ? 0 : -225;
        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * totalAtkSpeed);
        if (Mathf.Abs(t - target.z) < 1 && _swinging)
        {
            //_swing *= -1; // Double Swing
            _swinging = false;
        }
        transform.localRotation = Quaternion.Euler(target);

    }

    private void _getSwingAngle()
    {
        _swingAngle = Mathf.Lerp(_swingAngle, _swing * (angleOfTheWeapon), Time.deltaTime * (currentCombo == 0 ? totalAtkSpeed * 0.1f : totalAtkSpeed));
    }

    #endregion

    #region Weapon Thrust Functions

    private void _setThrustSpeed()
    {
        thrustSpeed = totalAtkSpeed / 2;
    }

    private void _setThrustingPosition()
    {
        _setThrustSpeed();
        _swingAngle = 0;
        transform.localPosition = new Vector2(0, 0.3f);
        Sprite.transform.localPosition = new Vector2(0, 0.3f);
    }

    private void _calculateWeaponThrustTrajectory()
    {
        _targetThrustPosition = new Vector2(0, thrustDistance);
        Vector2 defaultThrustPosition = new Vector2(0, 0.3f);
        _currentThrustPosition = Mathf.MoveTowards(_currentThrustPosition, _thrust, thrustSpeed * Time.deltaTime);
        Sprite.gameObject.transform.localPosition = Vector2.Lerp(defaultThrustPosition, _targetThrustPosition, _currentThrustPosition);
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
        if (weaponPosition == _targetThrustPosition && _thrusting)
        {
            _thrust = 0;
        }
    }

    private void _thrustWeapon()
    {
        if (_thrusting) return;

        _thrust = _thrust == 0f ? 1 : 0f;
        playRandomMeleeSwing();
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


    // In case there would be different weapons that would need this
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

    #region Audio Functions
    private void playRandomMeleeSwing()
    {
        audioSource.clip = WeaponSwingAudio[Random.Range(0, WeaponSwingAudio.Length)];
        audioSource.volume = Random.Range(1 - volumeChangeMultiplier, 1);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        audioSource.Play();
    }

    #endregion
    private void Attack(InputAction.CallbackContext context)
    {
        _attackComboHandler();
    }


}
