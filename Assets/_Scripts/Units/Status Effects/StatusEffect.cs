using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect 
{
    public abstract string GiveName();

}

public class BurningStatus : StatusEffect
{
    public override string GiveName()
    {
        return "Burning";
    }
}

public class FreezingStatus : StatusEffect
{
    public override string GiveName()
    {
        return "Freezing";
    }
}