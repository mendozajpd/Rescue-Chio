using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectApplier : MonoBehaviour
{
    private Attack _attack;
    private void Awake()
    {
        _attack = GetComponent<Attack>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        SetStatusEffectNames();
    }

    private void SetStatusEffectNames()
    {
        foreach (StatusEffectList s in _attack.statuseffects)
        {
            switch (s.effect)
            {
                case StatusEffects.Burning:
                    s.statusEffect = new BurningStatus();
                    s.name = s.statusEffect.GiveName();
                    s.tier = s.statusEffect.StartingTier();
                    s.duration = s.statusEffect.DefaultDuration();
                    break;
                case StatusEffects.Freezing:
                    s.statusEffect = new FreezingStatus();
                    s.name = s.statusEffect.GiveName();
                    break;
                default:
                    return;
            }
        }
    }


    public void AddStatusEffect(StatusEffects effect)
    {
        foreach (StatusEffectList s in _attack.statuseffects)
        {
            if (s.effect == effect)
            {
                s.tier += 1;
                return;
            }
        }
        _attack.statuseffects.Add(new StatusEffectList(effect));
        SetStatusEffectNames();
    }
}


public enum StatusEffects
{
    Burning,
    Freezing,
    Bleeding
}
