using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectList
{
    public StatusEffect statusEffect;
    public string name;
    public StatusEffects effect;

    public StatusEffectList(StatusEffects newEffect)
    {
        effect = newEffect;
    }

}
