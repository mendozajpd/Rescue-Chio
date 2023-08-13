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

    protected void DealDamage(Health health, StatsManager stats, Attack attack, float damage, Vector2 knockbackSource, float iTime, bool isCrit, bool inflictKB)
    {
        var unitStats = health.GetComponent<StatsManager>();
        health?.Damage(unitStats.CalculateFinalDamage(damage, isCrit), isCrit, iTime, attack);
        if(inflictKB) health?.InflictKnocback(knockbackSource, stats.CalculateTotalKnockback(unitStats.TotalKnockbackResistance), isCrit);
    }

    protected void AttackHitsEnemy(Collider2D collision, StatsManager attackerStats, float damage, Vector2 knockbackSource, float iTime)
    {
        var Enemy = collision.GetComponent<EnemyManager>();

        if (Enemy != null)
        {
            // ADD PARTICLES ON ENEMY POSITION TO INDICATE A HIT
            var EnemyHealth = Enemy.GetComponent<Health>();
            bool isInvincible = EnemyHealth.Invincible;
            if (!isInvincible)
            {
                bool isCrit = attackerStats.isCriticalStrike();
                DealDamage(EnemyHealth, attackerStats, this, damage, knockbackSource, iTime, isCrit, inflictsKnockback);
            }
        }
    }
}
