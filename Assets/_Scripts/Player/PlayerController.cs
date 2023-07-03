using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _sprite;
    private Rigidbody2D _rb;

    // Input System Variables
    [SerializeField] private PlayerControls _playerControls;
    [SerializeField] private InputAction _move;
    [SerializeField] private InputAction _dash;

    // MouseLocation
    private float _mouseLocation;
    private Vector2 _playerPos;
    private Vector2 _mousePos;
    private Vector2 _mouseDir;

    // Movespeed related
    [SerializeField] private float _moveSpeed;
    // Direction
    private Vector2 moveDirection;

    // Dash Variables
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashSpeed;
    private Vector2 _dashDirection;
    private float _dashCooldownTime;
    private float _dashTime;


    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        moveDirection = _move.ReadValue<Vector2>();

        anim_RunHandler();
        _spriteFlipper();
        _dashHandler();


    }

    private void anim_RunHandler()
    {
        if (moveDirection.magnitude > 0)
        {
            _anim.SetBool("isRunning", true);
        }
        else
        {
            _anim.SetBool("isRunning", false);
        }
    }

    private void _spriteFlipper()
    {
        _getMousePosition();

        if (_dashTime <= 0 )
        {
            if (_mouseLocation > 0)
            {
                _sprite.flipX = true;
            }

            if (_mouseLocation < 0)
            {
                _sprite.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_dashTime <= 0)
        {
            _rb.velocity = new Vector2(moveDirection.x * _moveSpeed, moveDirection.y * _moveSpeed);
        } else
        {
            _dashMovement();
        }

    }

    #region InputControls
    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _dash = _playerControls.Player.Dash;
        _move.Enable();
        _dash.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
        _dash.Disable();
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
    private void _dashDurationTimer()
    {
        if (_dashTime > 0)
        {
            _dashTime -= Time.deltaTime;
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

        if (_dashTime <= 0)
        {
            _dashCooldownTimer();
        }

        if (_dash.triggered && _dashTime <= 0 && _dashCooldownTime <= 0 )
        {
            if (_move.ReadValue<Vector2>().magnitude > 0)
            {
                _dashDirection = _move.ReadValue<Vector2>().normalized;
            } else
            {
                _dashDirection = _mouseDir.normalized;
            }

            _dashTime = _dashDuration;
        }

        if (_dashTime > 0)
        {
            _anim.SetBool("isDashing", true);
        }

        if (_dashTime <= 0 && _anim.GetBool("isDashing"))
        {
            _dashCooldownTime = _dashCooldown;
            _anim.SetBool("isDashing", false);
        }

    }

    private void _dashMovement()
    {
        if (_dashTime > 0)
        {
            _rb.velocity = _dashDirection * _dashSpeed;
        }
    }
    #endregion
}
