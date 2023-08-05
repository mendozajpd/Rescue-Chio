using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health),typeof(StatusEffectsManager))]
public class Player : Unit
{

    private void Awake()
    {
        SpawnRequiredComponents(UsesMana);
        UnitHealth.SetMaxHealth(100);
    }
}
