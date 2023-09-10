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

    public virtual void OnRemove(PowerupsManager unit, int stack)
    {
    }

    public virtual void OnPowerupPickup(PowerupsManager unit)
    {
    }

    public virtual float GiveStatAmount(float statAmount)
    {
        return statAmount;
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
public class HealthRegenPowerup : Powerup
{
    private float _regenAmount = 1;
    private Health _unitHealth;
    public override string GiveName()
    {
        return "Health Regen Powerup";
    }

    public override void OnPowerupPickup(PowerupsManager unit)
    {
        _unitHealth = unit.GetComponent<Health>();
    }

    public override void Update(PowerupsManager unit, int stack)
    {
        _unitHealth.Heal(_regenAmount * stack, false);
    }
}

public class DamagePowerup : Powerup
{
    private float _statAmount = 5;
    public override string GiveName()
    {
        return "Damage Powerup";
    }

    public override void OnPowerupPickup(PowerupsManager unit)
    {
        unit.TotalBonusDamage += _statAmount;
    }


    public override void OnRemove(PowerupsManager unit, int stack)
    {
        unit.TotalBonusDamage -= _statAmount;
    }
}


public class DefensePowerup : Powerup
{
    private float _statAmount = 3;
    public override string GiveName()
    {
        return "Defense Powerup";
    }

    public override void OnPowerupPickup(PowerupsManager unit)
    {
        unit.TotalBonusDefense += _statAmount;
    }

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        unit.TotalBonusDefense -= _statAmount;
    }
}

public class AttackSpeedPowerup : Powerup
{
    private float _statAmount = 50;

    public override string GiveName()
    {
        return "Attack Speed Powerup";
    }

    public override void OnPowerupPickup(PowerupsManager unit)
    {
        unit.TotalBonusAttackSpeed += _statAmount;
    }

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        unit.TotalBonusAttackSpeed -= _statAmount;
    }
}
public class MoveSpeedPowerup : Powerup
{
    private float _statAmount = 5;

    public override string GiveName()
    {
        return "Move Speed Powerup";
    }

    public override void OnPowerupPickup(PowerupsManager unit)
    {
        unit.TotalBonusMovementSpeed += _statAmount;
    }

    public override void OnRemove(PowerupsManager unit, int stack)
    {
        unit.TotalBonusMovementSpeed -= _statAmount;
        
    }
}
