using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected bool inflictsKnockback;

    public virtual void OnEnemyDeath(Health health)
    {
        Debug.Log("death message");
    }

    protected void PlayerDealDamageToEnemy(EnemyManager Enemy, Health health, StatsManager playerStats, Attack attack, float damage, Vector2 knockbackSource, float iTime, bool isCrit, bool inflictKB)
    {
        health?.Damage(Enemy.UnitStats.CalculateFinalDamageToEnemy(damage, isCrit), isCrit, iTime, attack);
        if(inflictKB) health?.InflictKnocback(knockbackSource, playerStats.CalculateTotalKnockback(Enemy.UnitStats.TotalKnockbackResistance), isCrit);
    }
}
