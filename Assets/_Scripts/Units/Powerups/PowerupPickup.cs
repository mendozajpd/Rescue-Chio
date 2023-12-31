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
                Collider2D col = GetComponent<Collider2D>();
                col.enabled = false;
                ParticleSystem particlesPrefab = Resources.Load<ParticleSystem>("Powerups/PowerupParticles");
                Instantiate(particlesPrefab, transform);
                StartCoroutine(destroyGameobject());
                AddPowerup(playerPowerupManager);
                powerup.OnPowerupPickup(playerPowerupManager);
            }
        }
    }

    IEnumerator destroyGameobject()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer?.gameObject?.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void AddPowerup(PowerupsManager unit)
    {
        foreach (PowerupList p in unit.powerups)
        {
            if (p.name == powerup.GiveName())
            {
                p.stack += 1;
                return;
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
            case Powerups.DefensePowerup:
                return new DefensePowerup();
            case Powerups.AttackSpeedPowerup:
                return new AttackSpeedPowerup();
            case Powerups.MoveSpeedPowerup:
                return new MoveSpeedPowerup();
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
    DefensePowerup,
    AttackSpeedPowerup,
    MoveSpeedPowerup
}
