using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupList
{
    public Powerup powerups;
    public string name;
    public int stack;

    public PowerupList(Powerup newPowerup, string powerupName, int newStack)
    {
        powerups = newPowerup;
        name = powerupName;
        stack = newStack;
    }
}
