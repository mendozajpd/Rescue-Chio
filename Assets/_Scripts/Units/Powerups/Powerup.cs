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

    public virtual float GiveStatAmount()
    {
        return 0;
    }
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
    private float _statAmount = 5;
    public override string GiveName()
    {
        return "Damage Powerup";
    }

    public override void OnPickUp(PowerupsManager unit, int stack)
    {
        unit.TotalBonusDamage += _statAmount;
    }

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        // Add the function that would destroy this
        // Remove the stat
        unit.TotalBonusDamage -= _statAmount;
    }
}

public class HealthRegenPowerup : Powerup
{
    private float _regenAmount = 1;
    private Health _unitHealth;

    public override string GiveName()
    {
        return "Health Regen Powerup";
    }

    public override void OnPickUp(PowerupsManager unit, int stack)
    {
        _unitHealth = unit.GetComponent<Health>();
    }

    public override void Update(PowerupsManager unit, int stack)
    {
        _unitHealth.Heal(_regenAmount * stack, false);
    }
}

public class DefensePowerup : Powerup
{
    private float _statAmount = 3;
    public override string GiveName()
    {
        return "Defense Powerup";
    }

    //public override void OnPickUp(PowerupsManager unit, int stack)
    //{
    //    unit.TotalBonusDefense += _statAmount;
    //}

    //public override float GiveStatAmount()
    //{
        
    //}

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        // Add the function that would destroy this
        // Remove the stat
        unit.TotalBonusDefense -= _statAmount;
    }
}
