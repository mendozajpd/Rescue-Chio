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
    private float _swingAngle;
    private float _angle;
    private float _swing = 1;

    // Type of Attack
    [SerializeField] private bool isUsingSwingAttack;
    [SerializeField] private bool isUsingThrustAttack;
    private int _currentWeapon = 1;


    // Weapon Direction Variables
    [SerializeField] private bool isLookingLeft;

    private bool swinging;

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
        _weaponSpriteFlipper();
        _weaponAttackHandler();
        _rotateWeaponAroundAnchor();
    }

    private void _weaponAttackHandler()
    {

        if (isUsingSwingAttack)
        {
            _getSwingAngle();
            _calculateWeaponSwingTrajectory();
        }


    }

    #region Weapon Swing Functions

    private void _setSwingingPosition()
    {
        transform.localPosition = new Vector2(0, 0.5f);
        Sprite.transform.localPosition = new Vector2(0.5f, 0.15f);
    }

    private void _swingWeapon()
    {
        if (swinging) return;

        // Attack
        _swing *= -1;
        swinging = true;
    }

    private void _calculateWeaponSwingTrajectory()
    {
        // Weapon Swing
        float t = _swing == 1 ? 0 : -225;
        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * totalAtkSpeed);
        if (Mathf.Abs(t - target.z) < 5 && swinging)
        {
            //_swing *= -1; // Double Swing
            swinging = false;
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
        transform.localPosition = new Vector2(0, 0.3f);
        Sprite.transform.localPosition = new Vector2(0, 0.3f);
    }

    #endregion

    #region Sprite Flipper
    private void _weaponSpriteFlipper()
    {
        // Looking Left
        if (_mousePos.x < 0)
        {
            //_flipSpriteLookingLeft();

            if (!isLookingLeft && !swinging)
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

            if (isLookingLeft && !swinging)
            {
                _swing = 1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = false;
            }

        }

    }



    private void _flipSpriteLookingRight()
    {
        if (_swing == 1 && !swinging)
        {
            Sprite.flipX = true;
        }
        else if (_swing == -1 && !swinging)
        {
            Sprite.flipX = false;
        }
    }

    private void _flipSpriteLookingLeft()
    {
        if (_swing == 1 && !swinging)
        {
            Sprite.flipX = false;
        }
        else if (_swing == -1 && !swinging)
        {
            Sprite.flipX = true;
        }
    }
    #endregion


    private void _rotateWeaponAroundAnchor()
    {
        _getMousePosition();
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle + _swingAngle;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Firee");

        if (isUsingSwingAttack)
        {
            _swingWeapon();
        }
    }

    private void ChangeCurrentAttack(InputAction.CallbackContext context)
    {
        if (!swinging)
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
