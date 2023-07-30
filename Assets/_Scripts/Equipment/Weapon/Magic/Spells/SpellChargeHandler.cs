using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellChargeHandler : MonoBehaviour
{
    private MagicWeapon _wand; 

    // get particle
    private ParticleSystem _particles;
    private ParticleSystem.EmissionModule _emission;

    private List<Spell> _spells = new List<Spell>();

    // divide the current charge to the max charge and put it inside the charge below
    [Header("Charge Variables")]
    [SerializeField] private float currentCharge;
    [SerializeField] private float chargeParticlesMultiplier = 100;

    // Input Actions
    private PlayerInputActions _playerControls;
    private InputAction _special;

    public List<Spell> Spells 
    { 
        get => _spells; 
        set
        {
            _spells = value;
            _giveSpellChargeHandlerToSpells();
        }
    }

    private void OnEnable()
    {
        _setPlayerInput();
    }

    private void OnDisable()
    {
        _special.performed -= _resetParticleCharge;
    }

    public float CurrentCharge 
    { 
        get => currentCharge; 
        set
        {
            currentCharge = value;
            _emission.rateOverTime = currentCharge * chargeParticlesMultiplier;
        }
    }

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _emission = _particles.emission;
        _wand = GetComponentInParent<MagicWeapon>();
    }
    void Start()
    {
        _giveSpellChargeHandlerToSpells();
    }

    void Update()
    {
        
    }

    private void _giveSpellChargeHandlerToSpells()
    {
        for (int i = 0; i < _spells.Count; i++)
        {
            _spells[_spells.Count - 1].SetSpellChargeHandler(this);
        }
    }

    private void _resetParticleCharge(InputAction.CallbackContext context)
    {
        _emission.rateOverTime = 0;
        Debug.Log("reset!");
    }

    private void _setPlayerInput()
    {
        _playerControls = new PlayerInputActions();
        _special = _playerControls.Player.Reload;
        _special.Enable(); 
        _special.performed += _resetParticleCharge;
    }

}
