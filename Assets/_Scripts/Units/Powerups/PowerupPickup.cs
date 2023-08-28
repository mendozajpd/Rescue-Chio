using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public Powerup powerup;
    public Powerups powerupDrop;

    void Start()
    {
        powerup = AssignItem(powerupDrop);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager playerManager = collision.GetComponent<PlayerManager>();

        if (playerManager != null)
        {
            PowerupsManager playerPowerupManager = playerManager.UnitPowerups;

            if (playerPowerupManager != null)
            {
                AddPowerup(playerPowerupManager);
                playerPowerupManager.CallItemOnPickup();
                Destroy(gameObject);
            }
        }
    }

    public void AddPowerup(PowerupsManager unit)
    {
        foreach (PowerupList p in unit.powerups)
        {
            if (p.name == powerup.GiveName())
            {
                p.stack += 1;
                return; // Will prevent the code from running the code below
            }
        }
        unit.powerups.Add(new PowerupList(powerup, powerup.GiveName(), 1));
    }


    public Powerup AssignItem(Powerups powerupToAssign)
    {
        switch (powerupToAssign)
        {
            case Powerups.HealthRegenPowerup:
                return new HealthRegenPowerup();
            case Powerups.DamagePowerup:
                return new DamagePowerup();
            default:
                return new UnknownPowerup();
        }
    }
}

public enum Powerups
{
    UnknownPowerup,
    HealthRegenPowerup,
    DamagePowerup,
}
