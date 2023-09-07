using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{
    private PlayerManager _unit;
    private StatsManager _stats;
    private Animator anim;
    private SpriteRenderer sprite;

    // Input System Variables
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction dash;
    private InputAction fire; // add interact instead of fire

    // MouseLocation
    private float _mouseLocation;
    private Vector2 _playerPos;
    private Vector2 _mousePos;
    private Vector2 _mouseDir;

    // Direction
    private Vector2 moveDirection;

    // Dash Variables TEMPORARY - MIGHT ADD A INTERFACE 'IDASHABLE'
    [SerializeField] private DashStatsSO dashStats;
    public bool Dashing;
    private AudioClip _dashSound;
    private float _dashCooldown;
    private float _dashDuration;
    private float _dashSpeed;
    private Vector2 _dashDirection;
    private float _dashCooldownTime;
    private float _dashTime;

    public float DashTime 
    { 
        get => _dashTime;
        set 
        { 
            _dashTime = value;
            Dashing = _dashTime <= 0 ? false : true;
        } 
    }
    

    public DashStatsSO DashStats 
    { 
        get => dashStats;
        set
        {
            // Destroy previous particles
            dashStats = value;
            // Set new dashstats
            _setDashStats(dashStats);
            // Instantiate new dash particles
        }
    }


    // Dash Actions
    public Action hasDashed;
    public Action hasStoppedDashing;

    // Dash Animation Variables - SCALE PLAYER
    private float _currentValue;

    // Audio
    [SerializeField] private AudioSource audioSource;




    private void Awake()
    {
        playerControls = new PlayerInputActions();
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        _unit = GetComponent<PlayerManager>();
        _stats = GetComponent<StatsManager>();

        if (dashStats != null) _setDashStats(dashStats);
    }
    void Start()
    {
        
    }

    void Update()
    {
        anim_RunHandler();
        _spriteFlipper();

        if (dashStats != null)
        {
            _dashHandler();
        }
        IsKnockbacked = _unit.UnitHealth.Knockbacked;
    }


    private void anim_RunHandler()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection.magnitude > 0)
        {
            if(_rb.isKinematic) _rb.isKinematic = false;
            if (!IsMoving) IsMoving = true;
            anim.speed = _stats.TotalMoveSpeed <= _stats.DefaultMoveSpeed ? _stats.TotalMoveSpeed/ _stats.DefaultMoveSpeed : _stats.TotalMoveSpeed * (_stats.DefaultMoveSpeed/_stats.TotalMoveSpeed) / _stats.DefaultMoveSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            _rb.isKinematic = true;
            if (IsMoving) IsMoving = false;
            anim.speed = _stats.TotalMoveSpeed / _stats.TotalMoveSpeed;
            anim.SetBool("isRunning", false);
        }
    }

    private void _spriteFlipper()
    {
        _getMousePosition();

        if (DashTime <= 0 )
        {
            if (_mouseLocation > 0)
            {
                sprite.flipX = true;
            }

            if (_mouseLocation < 0)
            {
                sprite.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!Dashing && !IsKnockbacked)
        {
            _moveCharacter();
        }

        if (Dashing && !IsKnockbacked)
        {
            if (_rb.isKinematic) _rb.isKinematic = false;
            _dashMovement();
        }

    }

    private void _moveCharacter()
    {
        _rb.velocity = new Vector2(moveDirection.x * _stats.TotalMoveSpeed, moveDirection.y * _stats.TotalMoveSpeed);
    }

    #region InputControls
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        dash = playerControls.Player.Dash;
        //fire = playerControls.Player.Fire;

        move.Enable();
        dash.Enable();
        //fire.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
        //dash.Disable();
    }
    #endregion

    private void _getMousePosition()
    {
        _playerPos = transform.position;
        _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _mouseDir = _mousePos - _playerPos;

        _mouseLocation = _mouseDir.x / Mathf.Abs(_mouseDir.x);
    }

    #region Dash

    private void _setDashStats(DashStatsSO DashStats)
    {
        _dashCooldown = DashStats.DashCooldown;
        _dashDuration = DashStats.DashDuration;
        _dashSpeed = DashStats.DashSpeed;
        _dashSound = DashStats.DashSound;
        audioSource.clip = _dashSound;
        Instantiate(DashStats.DashParticles, gameObject.transform);
    }
    private void _dashDurationTimer()
    {
        if (DashTime > 0)
        {
            DashTime -= Time.deltaTime;
        }
    }

    private void _dashCooldownTimer()
    {
        if(_dashCooldownTime > 0)
        {
            _dashCooldownTime -= Time.deltaTime;
        }
    }

    private void _dashHandler()
    {
        _dashDurationTimer();

        if (DashTime <= 0)
        {
            _dashCooldownTimer();
        }

        if (dash.triggered && DashTime <= 0 && _dashCooldownTime <= 0 )
        {
            if (move.ReadValue<Vector2>().magnitude > 0)
            {
                _dashDirection = move.ReadValue<Vector2>().normalized;
            } else
            {
                _dashDirection = _mouseDir.normalized;
            }

            if (dashStats.hasAnim) _currentValue = dashStats.baseValue;
            audioSource.Play();
            hasDashed?.Invoke();
            DashTime = _dashDuration;
        }

        if (DashTime > 0)
        {
            anim.SetBool("isDashing", true);
            if (dashStats.hasAnim)
            {
                _dashAnimation();
            }
        }

        if (DashTime <= 0 && anim.GetBool("isDashing"))
        {
            hasStoppedDashing?.Invoke();
            _dashCooldownTime = _dashCooldown;
            anim.SetBool("isDashing", false);
        }

    }

    private void _dashMovement()
    {
        if (DashTime > 0)
        {
            _rb.velocity = _dashDirection * _dashSpeed * (_stats.TotalMoveSpeed * 0.2f);
        }
    }

    private void _dashAnimation()
    {
        _currentValue = Mathf.MoveTowards(_currentValue, dashStats.targetValue, dashStats.lerpSpeed * Time.deltaTime);

        sprite.gameObject.transform.localScale = new Vector2(sprite.gameObject.transform.localScale.x, _currentValue);

    }
    #endregion
}
