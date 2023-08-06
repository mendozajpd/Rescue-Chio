using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitManager : MonoBehaviour
{
    // Health
    public Health UnitHealth;

    // Mana
    public bool UsesMana;
    public Mana UnitMana;

    // Status Effects
    public StatusEffectsManager UnitStatusEffects;


    public void SpawnRequiredComponents(bool usesMana)
    {
        UnitHealth = GetComponent<Health>();
        UnitStatusEffects = GetComponent<StatusEffectsManager>();
        if (usesMana) UnitMana = gameObject.AddComponent<Mana>();
    }

    public void DestroyRequiredComponents()
    {

    }
}
