using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float BaseDamage;
    public float UseTime;

    public void UseTimer()
    {
        if (UseTime > 0)
        {
            UseTime -= Time.deltaTime;
        }
    }
}
