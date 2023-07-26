using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicWeapon : Weapon
{
    [Header("Spells")]
    private SpellHandler spellHandler;
    public List<Spell> Spells = new List<Spell>(2);


    // Casts
    [SerializeField] private int numberOfCasts;

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
    public bool isLookingLeft;

    // Mouse Position Variables
    public Vector2 MouseAttackPosition;

    [SerializeField] private int currentSpellIndex = 0;

    // Wand Mechanics
    private bool _canSwing = false;
    // Can Swing
    // Is Point

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
            } else
            {
                currentSpellIndex = 0;
            }
            SetWandActions();
        }
    }

    // Temp
    private InputAction changeSpell;

    private void OnEnable()
    {
        Fire.Enable();
        Fire.performed += CastMagic;


        // Cycle through spells TEMPORARY
        changeSpell = playerControls.Player.Reload;
        changeSpell.Enable();
        changeSpell.performed += _cycleThroughSpells;

        spellHandler.UpdateCurrentSpells += _handleSpells;
    }

    private void OnDisable()
    {
        spellHandler.UpdateCurrentSpells -= _handleSpells;


        // Cycle through spells TEMPORARY
        changeSpell.performed -= _cycleThroughSpells;
        changeSpell.Disable();

        Fire.performed -= CastMagic;
        Fire.Disable();
    }
    private void Awake()
    {
        _anchor = gameObject;

        // Spell Handler
        spellHandler = GetComponentInChildren<SpellHandler>();

        SetInputVariables();
        SetSpriteVariables();
    }

    void Start()
    {

    }

    void Update()
    {
        GetMouseAttackPosition();
        _weaponSpriteFlipper();
        _getSwingAngle();
        _calculateWeaponSwingTrajectory();
        _rotateWeaponAroundAnchor();
    }

    private void _useWand()
    {
        // Attack
        if (_canSwing)
        {
            if (_swinging) return;
            _castSpell();
            _swingWand();
        }
        
        if (!_canSwing)
        {
            _castSpell();
            Debug.Log("Spell casted lmao");
        }
    }



    #region Spell Handling Functions
    private void _handleSpells()
    {
        Spells.Clear();
        for(int i = 0; i < spellHandler.CurrentNumberOfSpells; i++)
        {
            Spell spell = spellHandler.transform.GetChild(i).GetComponent<Spell>();
            Spells.Add(spell);
        }
        SetWandActions();
        Debug.Log("Updated current spells!");
    }

    private void _castSpell()
    {
        if (Spells.Count > 0)
        {
            for (int i = 0; i < numberOfCasts; i++)
            {
                Spells[currentSpellIndex]?.CastSpell();
            }
        }
    }

    #endregion

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

    public Vector2 GetMouseAttackPosition()
    {
        var mainCamera = Camera.main;
        MouseAttackPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return MouseAttackPosition;
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
        _useWand();
    }

    public void SetWandActions()
    {
        Spell currentSpell = Spells[currentSpellIndex];
        _canSwing = currentSpell.CanSwing;
        angleOfTheWeapon = currentSpell.WeaponAngle;

        // Resets swing
        _swing = 1;
        // Can also set wand position

    }

    private void _cycleThroughSpells(InputAction.CallbackContext context)
    {
        CurrentSpellIndex += 1;
    }

}

