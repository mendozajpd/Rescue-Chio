using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect 
{
    public abstract string GiveName();

    public virtual void InflictStatusEffect(StatusEffectsManager unit)
    {

    }

}

public class BurningStatus : StatusEffect
{
    public override string GiveName()
    {
        return "Burning";
    }

    public override void InflictStatusEffect(StatusEffectsManager unit)
    {
        unit.InflictBurningStatus(3.5f, 1);
    }
}

public class FreezingStatus : StatusEffect
{
    public override string GiveName()
    {
        return "Freezing";
    }
}