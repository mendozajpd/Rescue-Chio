using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public virtual void OnEnemyDeath(Health health)
    {
        Debug.Log("death message");
    }
}
