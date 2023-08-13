using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicWeapon : Weapon
{
    [Header("Spells")]
    private SpellHandler spellHandler;
    public List<Spell> Spells = new List<Spell>();
    public SpellChargeHandler SpellCharge;
    public Vector3 PlayerPos;

    [Header("Casts")]
    [SerializeField] private int numberOfCasts;

    // Weapon Rotation Variables
    [SerializeField] private float totalAtkSpeed;
    [SerializeField] private float weaponAngle;
    private GameObject _anchor;
    private Vector2 _mousePos;
    private float _angle;

    [Header("Swing Variables")]
    [SerializeField] private float angleOfTheWeapon;
    private float _swingAngle;
    private float _swing = 1;
    private bool _swinging;
    private Vector3 target;

    // Weapon Sprite Flip Functions
    public bool IsLookingLeft;
    // Mouse Position Variables
    public Vector2 MouseWorldPosition;

    [SerializeField] private int currentSpellIndex = 0;

    [Header("Wand Mechanics")]
    private bool _canSwingWeapon = false;
    [SerializeField] private bool _canRotateWeapon;


    public System.Action castTrigger;

    public float Swing { get => _swing; }
    public Vector2 MousePos { get => _mousePos; }
    public int CurrentSpellIndex
    {
        get => currentSpellIndex;
        set
        {
            if (currentSpellIndex < Spells.Count - 1)
            {
                currentSpellIndex = value;
            }
            else
            {
                currentSpellIndex = 0;
            }
            SetWandActions();
            _enableCurrentSpell();
            equipment.SetCurrentWeaponBaseDamage(BaseWeaponDamage);
        }
    }

    // Temp
    private InputAction _changeSpell;

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += CastMagic;



        // Cycle through spells
        _changeSpell = playerControls.Player.Special;
        _changeSpell.Enable();
        _changeSpell.performed += _cycleThroughSpells;

        spellHandler.UpdateCurrentSpells += _handleSpells;

    }

    private void OnDisable()
    {
        spellHandler.UpdateCurrentSpells -= _handleSpells;


        // Cycle through spells TEMPORARY
        _changeSpell.performed -= _cycleThroughSpells;
        _changeSpell.Disable();

        Fire.performed -= CastMagic;
        Fire.Disable();
    }
    private void Awake()
    {
        _anchor = gameObject;
        equipment = GetComponentInParent<PlayerEquipment>(); // This gets 

        // Spell Handler
        spellHandler = GetComponentInChildren<SpellHandler>();
        SpellCharge = GetComponentInChildren<SpellChargeHandler>();

        SetInputVariables();
        SetSpriteVariables();

        SetTotalWeaponDamage(equipment);
    }

    void Start()
    {
        equipment.SetCurrentWeaponBaseDamage(BaseWeaponDamage);

    }

    void Update()
    {
        GetMouseWorldPosition();
        _weaponSpriteFlipper();
        _getSwingAngle();
        _calculateWeaponSwingTrajectory();
        _rotateWeaponAroundAnchor();
        PlayerPos = GetComponentInParent<PlayerController>().transform.position;

    }

    private void _useWand()
    {
        // Attack
        if (_canSwingWeapon)
        {
            if (_swinging) return;
            _castSpell();
            _swingWand();
        }

        if (!_canSwingWeapon)
        {
            _castSpell();
        }
    }



    #region Spell Handling Functions
    private void _handleSpells()
    {
        Spells.Clear();
        for (int i = 0; i < spellHandler.CurrentNumberOfSpells; i++)
        {
            Spell spell = spellHandler.transform.GetChild(i).GetComponent<Spell>();
            Spells.Add(spell);
        }
        SetWandActions();
        _enableCurrentSpell();
        Debug.Log("Updated current spells!");

    }

    private void _enableCurrentSpell()
    {
        for (int i = 0; i < Spells.Count; i++)
        {
            if (i != currentSpellIndex)
            {
                Spells[i].gameObject.SetActive(false);
            }
            else
            {
                Spells[i].gameObject.SetActive(true);
                SetSpellStats(i);
            }
        }
    }

    private void SetSpellStats(int i)
    {
        BaseWeaponDamage = Spells[i].SpellDamage;
        SetTotalWeaponDamage(equipment);
        Spells[i].TotalSpellDamage = TotalWeaponDamage;

        BaseWeaponKnockback = Spells[i].SpellKnockback;
        SetTotalWeaponKnockback(equipment);
        Spells[i].TotalSpellKnockback = TotalWeaponKnockback;
        equipment.GetWeaponKnocback(TotalWeaponKnockback);
        equipment.SetCurrentWeaponBaseDamage(BaseWeaponDamage);

    }

    private void _castSpell()
    {
        if (Spells.Count > 0)
        {
            for (int i = 0; i < numberOfCasts; i++)
            {
                Spells[currentSpellIndex]?.CastSpell();
                castTrigger?.Invoke();
            }
        }
    }

    #endregion

    #region Weapon Rotation and Position

    private void _rotateWeaponAroundAnchor()
    {
        _getMousePosition();
        _angle = _canRotateWeapon ? (Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg) - weaponAngle : 0;
        Vector3 rotation = _anchor.transform.eulerAngles;
        rotation.z = _angle + _swingAngle;
        _anchor.transform.eulerAngles = rotation;
    }

    private void _getMousePosition()
    {
        _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(_anchor.transform.position);
    }

    public Vector2 GetMouseWorldPosition()
    {
        var mainCamera = Camera.main;
        MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return MouseWorldPosition;
    }

    #endregion

    #region Weapon Swing Functions
    private void _setSwingingPosition()
    {
        transform.localPosition = new Vector2(0, 0.5f);
        Sprite.transform.localPosition = new Vector2(0.5f, 0.15f);
    }

    private void _swingWand()
    {
        if (_swinging) return;

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

            if (!IsLookingLeft && !_swinging)
            {
                _swing = -1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                IsLookingLeft = true;
            }


        }


        // Looking Right
        if (_mousePos.x > 0)
        {
            //_flipSpriteLookingRight();

            if (IsLookingLeft && !_swinging)
            {
                _swing = 1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                IsLookingLeft = false;
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
        _useWand();
    }


    public void SetWandActions()
    {
        if (Spells.Count > 0)
        {
            Spell currentSpell = Spells[currentSpellIndex];
            _canSwingWeapon = currentSpell.CanSwing;
            angleOfTheWeapon = currentSpell.WeaponAngle;
            _canRotateWeapon = currentSpell.CanRotate;

            // Resets swing
            _swing = IsLookingLeft ? -1 : 1;
            // Can also set wand position
        }
        else
        {
            _setDefaultWandActions();
        }

    }

    private void _setDefaultWandActions()
    {
        currentSpellIndex = 0;
        _canSwingWeapon = true;
        angleOfTheWeapon = 90;
        _canRotateWeapon = true;
    }

    private void _cycleThroughSpells(InputAction.CallbackContext context)
    {
        CurrentSpellIndex += 1;

    }





}

