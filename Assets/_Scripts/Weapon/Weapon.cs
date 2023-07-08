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


    private void Awake()
    {

    }

    public void UseTimer()
    {
        if (UseTime > 0)
        {
            UseTime -= Time.deltaTime;
        }
    }



}
