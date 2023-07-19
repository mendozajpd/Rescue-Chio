using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicWeapon : Weapon
{

    // Weapon Rotation Variables
    [SerializeField] private float totalAtkSpeed;
    [SerializeField] private float weaponAngle;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private float _angle;

    // Swing Variables
    [SerializeField] private float angleOfTheWeapon;
    private float _swingAngle;
    private float _swing = 1;
    private bool _swinging;
    private Vector3 target;

    // Weapon Sprite Flip Functions
    [SerializeField] private bool isLookingLeft;

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += CastMagic;
    }

    private void OnDisable()
    {
        Fire.performed -= CastMagic;
        Fire.Disable();
    }
    private void Awake()
    {
        _anchor = gameObject;

        SetInputVariables();
        SetSpriteVariables();
    }

    void Start()
    {

    }

    void Update()
    {
        _weaponSpriteFlipper();
        _getSwingAngle();
        _calculateWeaponSwingTrajectory();
        _rotateWeaponAroundAnchor();
    }



    #region Weapon Rotation and Position

    private void _rotateWeaponAroundAnchor()
    {
        _getMousePosition();
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle + _swingAngle ;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
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
        if (Mathf.Abs(t - target.z) < 1 && _swinging)
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

    #region Not Used
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

    #endregion


    private void CastMagic(InputAction.CallbackContext context)
    {
        _swingWeapon();
    }

}

