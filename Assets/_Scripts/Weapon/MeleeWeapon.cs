using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{


    // Weapon Rotation Variables
    [SerializeField] private float totalAtkSpeed;
    [SerializeField] private float weaponAngle;
    private float _angleRotation;
    private float _angleOfTheWeapon = -90;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private Vector3 target;
    private float _swingAngle;
    private float _angle;
    private float _swing = 1;

    // Weapon Flipping Variables
    [SerializeField] private float weaponScaleX;
    [SerializeField] private bool meleeIsFlipped;


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
        if (!swinging)
        {
            _weaponFlipper();
        }
        _swingWeapon();
        _rotateWeaponAroundAnchor();
    }


    private void _swingWeapon()
    {
        // Weapon Swing
        _swingAngle = Mathf.Lerp(_swingAngle, _swing * (_angleOfTheWeapon), Time.deltaTime * totalAtkSpeed);
        float t = _swing == 1 ? 0 : -225;
        target.z = Mathf.Lerp(target.z, t, Time.deltaTime * totalAtkSpeed);
        if (Mathf.Abs(t - target.z) < 5 && swinging) 
        {
            swinging = false;
        }
        transform.localRotation = Quaternion.Euler(target);


    }

    private void _rotateWeaponAroundAnchor()
    {
        _getMousePosition();
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle + _swingAngle;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _weaponFlipper()
    {
        weaponScaleX = _mousePos.x < 0 ? -1 : 1;
        // when looking left
        if (_mousePos.x < 0 && !meleeIsFlipped)
        {
            // flip weapon
            sprite.flipX = false;
            // move position of weapon gameobject
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            // change the swing of rotation of the swing
            //_swing *= 1;

            meleeIsFlipped = true;
        }

        // looking right
        if (_mousePos.x > 0 && meleeIsFlipped)
        {
            sprite.flipX = true;
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            meleeIsFlipped = false;
        }
    }


    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Firee");

        if (swinging) return;

        // Attack
        _swing *= -1;
        swinging = true;
    }

}
