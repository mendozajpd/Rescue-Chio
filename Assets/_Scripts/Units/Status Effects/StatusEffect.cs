using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect 
{
    public abstract float DefaultDuration();
    public abstract int StartingTier();
    public abstract string GiveName();

    public virtual void InflictStatusEffect(StatusEffectsManager unit, float duration, int tier)
    {

    }

}

public class BurningStatus : StatusEffect
{
    public float _duration = 3.5f;
    public int _tier = 1;
    public override float DefaultDuration ()
    { 
        return _duration;
    }
    public override int StartingTier()
    {
        return _tier;
    }
    public override string GiveName()
    {
        return "Burning";
    }

    public override void InflictStatusEffect(StatusEffectsManager unit, float duration, int tier)
    {
        unit.InflictBurningStatus(duration, tier);
    }
}

public class FreezingStatus : StatusEffect
{
    private float _duration = 2.5f;
    private int _tier = 1;

    public override string GiveName()
    {
        return "Freezing";
    }

    public override float DefaultDuration()
    {
        return _duration;
    }
    public override int StartingTier()
    {
        return _tier;
    }
}