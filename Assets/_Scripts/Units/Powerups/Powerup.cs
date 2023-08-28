using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Powerup
{
    public abstract string GiveName();
    public virtual void Update(PowerupsManager unit, int stack)
    {
    }

    public virtual void OnPickUp(PowerupsManager unit, int stack)
    {
    }

    public virtual void OnRemove(PowerupsManager unit, int stack)
    {
    }

    public virtual void OnStatUpdate(PowerupsManager unit, int stack)
    {
    }

    //public virtual void AddStats(PowerupsManager unit, int stack)
    //{

    //}
}

public class UnknownPowerup : Powerup
{
    public override string GiveName()
    {
        return "Unknown Powerup";
    }

    // this does nothing
}

public class DamagePowerup : Powerup
{
    public override string GiveName()
    {
        return "Damage Powerup";
    }

    public override void OnPickUp(PowerupsManager unit, int stack)
    {
        unit.TotalBonusDamage += 5;
    }

    //public override void OnStatUpdate(PowerupsManager unit, int stack)
    //{
    //    unit.TotalBonusDamage += 5;
    //}

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        unit.TotalBonusDamage -= 5;
    }
}

public class HealthRegenPowerup : Powerup
{
    public override string GiveName()
    {
        return "Health Regen Powerup";
    }

    public override void Update(PowerupsManager unit, int stack)
    {
        // do regen stuff here
    }
}
