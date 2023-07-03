using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction _playerControls;
    [SerializeField] private Animator _anim;
    private Rigidbody2D _rb;
    


    // Movespeed related
    [SerializeField] private float _moveSpeed;
    // Direction
    private Vector2 moveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();

    }
    void Start()
    {
        
    }

    void Update()
    {
        moveDirection = _playerControls.ReadValue<Vector2>();

        if (moveDirection.magnitude > 0)
        {
            _anim.SetBool("isRunning", true);
        } else
        {
            _anim.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(moveDirection.x * _moveSpeed, moveDirection.y * _moveSpeed);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }


}
