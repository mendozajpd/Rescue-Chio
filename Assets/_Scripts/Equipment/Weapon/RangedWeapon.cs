using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : Weapon
{

    // Weapon Rotation Variables
    [SerializeField] private float weaponAngle;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private float _angle;

    // Shoot Animation
    [SerializeField] private Quaternion targetRecoilPosition;
    [SerializeField] private float recoilAmount;
    [SerializeField] private float recoilRecoverySpeed;
    [SerializeField] private float currentGunPosition;
    [SerializeField] private float shoot;
    [SerializeField] private bool shooting;

    // Weapon Sprite Flipping Variables
    private bool isLookingLeft;

    // Weapon Event
    public Action shootTrigger;

    public bool IsLookingLeft { get => isLookingLeft; }

    private void Awake()
    {
        // Weapon Rotation Variable
        _anchor = transform.gameObject;

        // Weapon Sprite
        SetSpriteVariables();

        // Input Variables
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
        _shootAnimationHandler();
        _rotateWeaponAroundAnchor();
    }

    private void _attackHandler()
    {

    }

    private void _shootAnimationHandler()
    {
        targetRecoilPosition = Quaternion.Euler(0, 0, recoilAmount);
        Quaternion defaultGunPosition = Quaternion.Euler(Vector3.zero);
        currentGunPosition = Mathf.MoveTowards(currentGunPosition, shoot, recoilRecoverySpeed < 3 ? 3 : recoilRecoverySpeed * Time.deltaTime);
        Sprite.gameObject.transform.localRotation = Quaternion.Lerp(defaultGunPosition, targetRecoilPosition, currentGunPosition);
        Quaternion gunPositon = Sprite.gameObject.transform.localRotation;

        _recoverFromRecoil(gunPositon);
        _shootAnimationHandler(shooting);

        if (gunPositon == defaultGunPosition)
        {
            shooting = false;
        }
    }

    private void _recoverFromRecoil(Quaternion currentGunPos)
    {
        if (currentGunPos == targetRecoilPosition && shooting)
        {
            shoot = 0;
        }
    }

    private void _shootWeapon()
    {
        if (shooting) return;

        shoot = shoot == 0 ? 1 : 0;
        shootTrigger.Invoke();
        shooting = true;

    }

    #region Weapon Sprite Handler
    private void _shootAnimationHandler(bool isShoot)
    {
        switch (isShoot)
        {
            case true:
                Anim.SetBool("isShooting", true);
                break;
            case false:
                Anim.SetBool("isShooting", false);
                break;
        }

    }
    #endregion

    #region Weapon Rotation and Position

    private void _rotateWeaponAroundAnchor()
    {
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
    }

    #endregion

    private void _weaponSpriteFlipper()
    {
        // Looking Left
        if (_mousePos.x < 0)
        {

            if (!isLookingLeft)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = true;
            }


        }

        // Looking Right
        if (_mousePos.x > 0)
        {

            if (isLookingLeft)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                isLookingLeft = false;
            }

        }

    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Pewpew");
        _shootWeapon();
    }

}
