using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    // Common Weapon Variables
    protected float TotalDamage;
    public float BaseDamage;
    public float UseTime;

    //Input System Variables
    public PlayerInputActions playerControls;
    public InputAction Fire;
    public InputAction ChangeAttack;
    public InputAction HoldFire;

    // Weapon Sprite
    public SpriteRenderer Sprite;
    public Animator Anim;

    // Equipment
    public PlayerEquipment equipment;

    public void UseTimer()
    {
        if (UseTime > 0)
        {
            UseTime -= Time.deltaTime;
        }
    }

    protected void SetInputVariables()
    {
        playerControls = new PlayerInputActions();
        Fire = playerControls.Player.Fire;
        ChangeAttack = playerControls.Player.ChangeAttack;
        HoldFire = playerControls.Player.HoldFire;
    }

    protected void SetSpriteVariables()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponentInChildren<Animator>();
    }


    protected void SetTotalWeaponDamage(PlayerEquipment equipment)
    {
        TotalDamage = equipment.playerStats.CalculateTotalDamage(BaseDamage);
    }

}
