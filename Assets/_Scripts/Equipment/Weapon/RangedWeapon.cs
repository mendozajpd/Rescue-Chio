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

    // Ranged Weapon Variables
    [SerializeField] private float currentAmmo;
    [SerializeField] private float maxAmmo;
    [SerializeField] private bool isEmpty;

    // Reload Variables
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;

    // Weapon Sprite Flipping Variables
    private bool isLookingLeft;

    // Weapon Event
    public System.Action shootTrigger;
    public System.Action reloadTrigger;

    // Input System Variables
    public InputAction Reload;

    // Audio Variables
    public AudioClip[] weaponAudio;
    private AudioSource audioSource;
    [Range(0f, 0.5f)]
    public float volumeChangeMultiplier;
    [Range(0f, 0.5f)] 
    public float pitchChangeMultiplier;
    [Range(0,1)]
    public float gunShootVolume;

    public bool IsLookingLeft { get => isLookingLeft; }

    private void Awake()
    {
        // Weapon Rotation Variable
        _anchor = transform.gameObject;

        // Weapon Variables
        currentAmmo = maxAmmo;

        // Audio Variables
        audioSource = GetComponent<AudioSource>();

        // Weapon Sprite
        SetSpriteVariables();

        // Input Variables
        SetInputVariables();
        Reload = playerControls.Player.Reload;
    }

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += Attack;

        Reload.Enable();
        Reload.performed += ReloadWeapon;
    }

    private void OnDisable()
    {
        Fire.performed -= Attack;
        Fire.Disable();

        Reload.performed -= ReloadWeapon;
        Reload.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _getMousePosition();
        _weaponSpriteFlipper();
        _reloadTimer();
        _shootAnimationHandler();
        _rotateWeaponAroundAnchor();
    }

    private void _attackHandler()
    {
        _shootWeapon();
        // this is where to code the bullet and ammo logic
    }


    #region Shooting Functions
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
        weaponAudioHandler(1,true); // Reload clip will be at 0
        shooting = true;
        currentAmmo -= 1;

    }

    #endregion

    #region Reload Functions

    private void _reloadHandler()
    {
        // Reloads weapon
        reloadTrigger.Invoke();
        weaponAudioHandler(0,false);
        currentAmmo = maxAmmo;
    }

    private void _reloadTimer()
    {
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
        }
    }

    #endregion

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

    #region Weapon Rotation, Position, Flipping

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
    #endregion

    private void weaponAudioHandler(int clipNumber, bool isOneShot)
    {
        audioSource.clip = weaponAudio[clipNumber];
        audioSource.volume = Random.Range(gunShootVolume - volumeChangeMultiplier, gunShootVolume);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        playAudio(isOneShot);
    }

    private void playAudio(bool isOneShot)
    {
        if (isOneShot)
        {
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("The Length is:" + audioSource.clip.length);
            return;
        }
        audioSource.Play();
        Debug.Log("The Length is:" + audioSource.clip.length);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Pewpew");
        _attackHandler();
    }

    private void ReloadWeapon(InputAction.CallbackContext context)
    {
        Debug.Log("reloadin");
        _reloadHandler();
    }



}
