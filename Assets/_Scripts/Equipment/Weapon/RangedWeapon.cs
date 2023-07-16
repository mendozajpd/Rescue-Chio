using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : Weapon
{

    // Weapon Rotation Variables
    [SerializeField] private float weaponAngle;
    private GameObject _anchor;
    private float _angle;
    private Vector2 _mousePos;

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

    // Reload Variables
    public float ReloadSpeed;
    [SerializeField] private float reloadTime;
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
    //public System.Action pullReloadNotEmpty;
    //private System.Action releaseReload;

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
    public float AudioVolume;

    public bool IsLookingLeft { get => isLookingLeft; }
    public float ReloadTime { get => reloadTime; }

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

        magazine.magazineInserted += _pullReloadHandler;



    }

    private void OnDisable()
    {
        Fire.performed -= Attack;
        Fire.Disable();

        Reload.performed -= ReloadWeapon;
        Reload.Disable();

        magazine.magazineInserted -= _pullReloadHandler;
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
        if (isReloading) return;

        if (currentAmmo == 0)
        {
            _reloadHandler();
            return;
        }

        _shootWeapon();
        // this is where to code the bullet and ammo logic
    }


    #region Shooting Functions
    private void _shootAnimationHandler()
    {
        targetRecoilPosition = Quaternion.Euler(0, 0, recoilAmount);
        Quaternion defaultGunPosition = Quaternion.Euler(Vector3.zero);
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
        rangedWeaponAudioHandler(1,true); // Reload clip will be at 0
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

        // Reloads weapon
        reloadTrigger.Invoke();
        reload = 1;
        isReloading = true;
        rangedWeaponAudioHandler(0,false);
        reloadTime = ReloadSpeed;
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
        if(canPull)
        {
            if (reloadTime < ReloadSpeed * 0.4f)
            {
                _pullReloadAnim();
                canPull = false;
                canRelease = true;
            }

        }
        
        if (canRelease)
        {
            if (reloadTime < ReloadSpeed * 0.2f)
            {
                Debug.Log("gun released");
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
        Debug.Log("Pewpew");
        _attackHandler();
    }

    private void ReloadWeapon(InputAction.CallbackContext context)
    {
        Debug.Log("reloadin");
        _reloadHandler();
    }



}
