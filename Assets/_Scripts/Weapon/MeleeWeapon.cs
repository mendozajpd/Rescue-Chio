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


    // Weapon Direction Variables
    [SerializeField] private bool isLookingLeft;

    private bool swinging;

    // Weapon Sprite Variables
    [SerializeField] private SpriteRenderer sprite;


    private void Awake()
    {
        _anchor = gameObject.transform.gameObject;
        sprite = GetComponentInChildren<SpriteRenderer>();

        // Input System Variables

        playerControls = new PlayerInputActions();
        Fire = playerControls.Player.Fire;
    }

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += Attack;
    }

    private void OnDisable()
    {
        Fire.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        //_weaponSwingHandler();
        _rotateWeaponAroundAnchor();
    }

    private void _swingWeapon()
    {
        if (swinging) return;

        // Attack
        _swing *= -1;
        swinging = true;
    }

    private void _weaponSwingHandler()
    {
        _determineSwingDirection();

        // Weapon Swing
        _swingAngle = Mathf.Lerp(_swingAngle, _swing * (angleOfTheWeapon), Time.deltaTime * totalAtkSpeed);
        float t = _swing == 1 ? 0 : -225;
        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * totalAtkSpeed);
        if (Mathf.Abs(t - target.z) < 5 && swinging)
        {
            //_swing *= -1; // Double Swing
            swinging = false;
        }
        transform.localRotation = Quaternion.Euler(target);
        

    }

    private void _determineSwingDirection()
    {
        // Looking Left
        if (_mousePos.x < 0)
        {
            if (_swing == 1 && !swinging)
            {
                sprite.flipX = false;
            } else if (_swing == -1 && !swinging)
            {
                sprite.flipX = true;
            }

            if (!isLookingLeft)
            {
                _swing = -1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = true;
            }


        }


        // Looking Right
        if (_mousePos.x > 0)
        {
            if (_swing == 1 && !swinging)
            {
                sprite.flipX = true;
            }
            else if (_swing == -1 && !swinging)
            {
                sprite.flipX = false;
            }

            if (isLookingLeft)
            {
                _swing = 1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = false;
            }

        }

    }

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

        //_swingWeapon();
    }

}
