using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{

    // Common Weapon Variables
    public float BaseDamage;
    public float UseTime;

    //Input System Variables
    public PlayerInputActions playerControls;
    public InputAction Fire;
    public InputAction ChangeAttack;

    // Weapon Sprite
    public SpriteRenderer Sprite;
    public Animator Anim;


    public void UseTimer()
    {
        if (UseTime > 0)
        {
            UseTime -= Time.deltaTime;
        }
    }

    public void SetInputVariables()
    {
        playerControls = new PlayerInputActions();
        Fire = playerControls.Player.Fire;
        ChangeAttack = playerControls.Player.ChangeAttack;
    }

    public void SetSpriteVariables()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
    }



}
