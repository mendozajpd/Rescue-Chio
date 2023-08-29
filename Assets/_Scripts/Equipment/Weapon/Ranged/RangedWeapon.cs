using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : Weapon
{
    [Header("Weapon Variables")]
    [SerializeField] private float defaultBaseDamage;
    [SerializeField] private float defaultBaseKnockback;
    [SerializeField] private float defaultFireRate;

    private float _weaponAngle = 90;
    private GameObject _anchor;
    private float _angle;
    private Vector2 _mousePos;

    [Header("Ranged Weapon Stats")]
    [SerializeField] private float maxAmmo;
    [Range(0, 10)]
    [SerializeField] private float accuracy;
    [Range(0, 10)]
    [SerializeField] private float reloadSpeed;

    [Header("Shooting Related Variables")]
    [SerializeField] private Quaternion targetRecoilPosition;
    [SerializeField] private float recoilAmount;
    [SerializeField] private float recoilRecoverySpeed;
    [SerializeField] private float currentGunPosition;
    [SerializeField] private float shoot;
    [SerializeField] private bool shooting;

    [Header("Ranged Weapon Variables")]
    [SerializeField] private float currentAmmo;


    [Header("Reload Variables")]
    [SerializeField] private float reloadTime;
    [SerializeField] private float reloadSpeedDuration;
    [SerializeField] private bool canPull;
    [SerializeField] private bool canRelease;
    [SerializeField] private bool isReloading;
    [SerializeField] private float reload;

    // Magazine Variables
    [SerializeField] private PistolReload magazine;

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
    [Range(0, 1)]
    public float AudioVolume;

    public bool IsLookingLeft { get => isLookingLeft; }
    public float ReloadTime { get => reloadTime; }
    public float ReloadSpeedDuration { get => reloadSpeedDuration; set => reloadSpeedDuration = value; }

    #region Gun Stats
    public float Accuracy { get => accuracy; set => accuracy = value; }
    public float ReloadSpeed { get => reloadSpeed; set => reloadSpeed = value; }
    #endregion

    private void Awake()
    {
        // Weapon Rotation Variable
        _anchor = transform.gameObject;

        // Weapon Variables
        currentAmmo = maxAmmo;

        // Magazine Variable
        magazine = GetComponentInChildren<PistolReload>();

        // Audio Variables
        audioSource = GetComponent<AudioSource>();

        equipment = GetComponentInParent<PlayerEquipment>();

        // Weapon Sprite
        SetSpriteVariables();

        // Input Variables
        SetInputVariables();
        Reload = playerControls.Player.Special;

    }

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += Attack;

        Reload.Enable();
        Reload.performed += _reloadWeapon;

        magazine.magazineInserted += _pullReloadHandler;

        ActivateAutoFire(_fireWeapon);
    }



    private void OnDisable()
    {
        Fire.performed -= Attack;
        Fire.Disable();

        Reload.performed -= _reloadWeapon;
        Reload.Disable();

        DisableAutoFire(_fireWeapon);

        magazine.magazineInserted -= _pullReloadHandler;
    }



    void Start()
    {
        WeaponBaseDamage = defaultBaseDamage;
        WeaponBaseKnockback = defaultBaseKnockback;
        WeaponBaseAttackSpeed = defaultFireRate;
    }

    void Update()
    {
        _getMousePosition();
        _weaponSpriteFlipper();
        _reloadTimer();
        _shootAnimationHandler();
        _rotateWeaponAroundAnchor();
    }

    private void FixedUpdate()
    {
        UseTimer(equipment.playerStats.TotalAttackSpeed);
    }


    private void _attackHandler()
    {
        if (isReloading) return;

        if (currentAmmo == 0)
        {
            _reloadHandler();
            return;
        }

        _shootWeapon();
        useTime = UseTimeDuration;
        // this is where to code the bullet and ammo logic
    }


    #region Shooting Functions
    private void _shootAnimationHandler()
    {
        targetRecoilPosition = Quaternion.Euler(0, 0, recoilAmount);
        Quaternion defaultGunPosition = Quaternion.Euler(Vector3.zero);
        recoilRecoverySpeed = 20 + (equipment.playerStats.TotalAttackSpeed * 0.25f);
        if (!isReloading) currentGunPosition = Mathf.MoveTowards(currentGunPosition, shoot, recoilRecoverySpeed < 3 ? 3 : recoilRecoverySpeed * Time.deltaTime);
        Sprite.gameObject.transform.localRotation = Quaternion.Lerp(defaultGunPosition, targetRecoilPosition, currentGunPosition);
        Quaternion gunPositon = Sprite.gameObject.transform.localRotation;

        if (!isReloading) _recoverFromRecoil(gunPositon);

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
        _triggerShootAnim();
        rangedWeaponAudioHandler(1, true);
        shooting = true;
        currentAmmo -= 1;
        if (currentAmmo == 0)
        {
            Anim.SetTrigger("isEmpty");
        }
    }

    #endregion

    #region Reload Functions

    private void _reloadHandler()
    {
        if (isReloading) return;

        if (shooting) return;

        if (currentAmmo == maxAmmo) return;

        ReloadSpeedDuration = (10 - (ReloadSpeed - 1)) * 0.1f;
        reloadTrigger.Invoke();
        reload = 1;
        isReloading = true;
        rangedWeaponAudioHandler(0, false);
        reloadTime = ReloadSpeedDuration;
        //reloadTime = ReloadSpeed;
    }

    private void _reloadTimer()
    {
        if (isReloading)
        {
            currentGunPosition = Mathf.MoveTowards(currentGunPosition, reload, 30 * Time.deltaTime);
            _reloadAnimatorHandler();
            if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
            }

            if (reloadTime <= 0)
            {
                reloadTime = 0;
                isReloading = false;
            }
        }
    }


    private void _reloadAnimatorHandler()
    {
        if (canPull)
        {
            if (reloadTime < ReloadSpeedDuration * 0.4f)
            {
                _pullReloadAnim();
                canPull = false;
                canRelease = true;
            }

        }

        if (canRelease)
        {
            if (reloadTime < ReloadSpeedDuration * 0.2f)
            {
                _releaseReloadAnim();
                canRelease = false;
            }
        }



    }

    private void _pullReloadAnim()
    {

        Anim.SetTrigger("reloadPull");
        rangedWeaponAudioHandler(2, true);
        reload = 0;
    }

    private void _releaseReloadAnim()
    {
        Anim.SetTrigger("reloadRelease");
        rangedWeaponAudioHandler(3, true);
    }

    private void _pullReloadHandler()
    {
        currentAmmo = maxAmmo;
        canPull = true;
    }
    #endregion

    #region Weapon Sprite Handler
    private void _triggerShootAnim()
    {
        Anim.SetTrigger("hasShot");

    }
    #endregion

    #region Weapon Rotation, Position, Flipping

    private void _rotateWeaponAroundAnchor()
    {
        _angle = (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - _weaponAngle;
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

    private void rangedWeaponAudioHandler(int clipNumber, bool isOneShot)
    {
        audioSource.clip = weaponAudio[clipNumber];
        audioSource.volume = Random.Range(AudioVolume - volumeChangeMultiplier, AudioVolume);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        playAudio(isOneShot);
    }

    private void playAudio(bool isOneShot)
    {
        if (isOneShot)
        {
            audioSource.PlayOneShot(audioSource.clip);
            return;
        }
        audioSource.Play();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        _fireWeapon();
    }

    private void _fireWeapon()
    {
        if (useTime <= 0)
        {
            _attackHandler();
        }
    }

    private void _reloadWeapon(InputAction.CallbackContext context)
    {
        _reloadHandler();
    }



}
