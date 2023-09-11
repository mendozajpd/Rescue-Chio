using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatusEffectApplier))]
public abstract class Attack : MonoBehaviour
{

    public List<StatusEffectList> statuseffects = new List<StatusEffectList>();

    public virtual void OnEnemyDeath(Health health)
    {
        Debug.Log("death message");
    }

    private void InflictStatusEffects(Health receiver)
    {
        StatusEffectsManager statusReceiver = receiver.GetComponent<StatusEffectsManager>();

        foreach(StatusEffectList s in statuseffects)
        {
            s.statusEffect.InflictStatusEffect(statusReceiver);
        }
    }

    private void DealDamageAndKnockback(Health health, StatsManager attackerStats, Attack attack, float damage, Vector2 knockbackSource, float iTime, bool isCrit, bool inflictKB)
    {
        var damageReceiver = health.GetComponent<StatsManager>();
        health?.Damage(damageReceiver.CalculateFinalDamage(damage, isCrit), isCrit, iTime, attack, health.NormalAttackColor);
        if(inflictKB) health?.InflictKnocback(knockbackSource, attackerStats.CalculateTotalKnockback(damageReceiver.TotalKnockbackResistance), isCrit);
        InflictStatusEffects(health);
    }

    protected void TriggerDamageKnocbackEnemy(Collider2D collision, StatsManager attackerStats, Vector2 knockbackSource, float iTime, bool inflictsKB)
    {
        var Enemy = collision.GetComponent<EnemyManager>();

        if (Enemy != null)
        {
            // ADD PARTICLES ON ENEMY POSITION TO INDICATE A HIT
            var EnemyHealth = Enemy.GetComponent<Health>();
            bool isInvincible = EnemyHealth.Invincible;
            var totalDamage = attackerStats.TotalDamage;
            if (!isInvincible)
            {
                bool isCrit = attackerStats.isCriticalStrike();
                DealDamageAndKnockback(EnemyHealth, attackerStats, this, totalDamage, knockbackSource, iTime, isCrit, inflictsKB);
            }
        }
    }

    protected void CollisionDamageKnocbackEnemy(Collision2D collision, StatsManager attackerStats, Vector2 knockbackSource, float iTime, bool inflictsKB)
    {
        
        var Enemy = collision.gameObject.GetComponent<EnemyManager>();

        if (Enemy != null)
        {
            // ADD PARTICLES ON ENEMY POSITION TO INDICATE A HIT
            var EnemyHealth = Enemy.GetComponent<Health>();
            bool isInvincible = EnemyHealth.Invincible;
            var totalDamage = attackerStats.TotalDamage;
            if (!isInvincible)
            {
                bool isCrit = attackerStats.isCriticalStrike();
                DealDamageAndKnockback(EnemyHealth, attackerStats, this, totalDamage, knockbackSource, iTime, isCrit, inflictsKB);
            }
        }
    }
    protected void ExplosiveDamageKnocbackEnemy(Collider2D collision, StatsManager attackerStats, Vector2 knockbackSource, float iTime, float explosionRadius)
    {
        var Enemy = collision.GetComponent<EnemyManager>();
        var damagePoint = collision.ClosestPoint(transform.position);
        if (Enemy != null)
        {
            var EnemyHealth = Enemy.GetComponent<Health>();
            bool isInvincible = EnemyHealth.Invincible;
            var damageReceiver = EnemyHealth.GetComponent<StatsManager>();
            float distance = Vector2.Distance(transform.position, damagePoint);
            float distanceMultiplier = 1f / (1 + 0.075f * (distance * (explosionRadius * 2)));
            float totalDamage = attackerStats.TotalDamage * distanceMultiplier;
            float totalKnockbackReceived = attackerStats.CalculateTotalKnockback(damageReceiver.TotalKnockbackResistance) * distanceMultiplier;
            #region debugs
            //Debug.Log("distance: " + distance);
            //float knockbackOverDistance = 1 / (1 + 0.2f * (distance * (explosionRadius * 2)));
            //Debug.Log("damage multiplier: " + distanceMultiplier);
            //Debug.Log("knockback multiplier: " + distanceMultiplier);
            //Debug.Log("damage inflicted: " + totalDamage);
            //Debug.Log("knockback inflicted: " + totalKnockbackReceived); 
            #endregion
            if (!isInvincible)
            {
                bool isCrit = attackerStats.isCriticalStrike();
                EnemyHealth?.Damage(damageReceiver.CalculateFinalDamage(totalDamage, isCrit), isCrit, iTime, this, EnemyHealth.NormalAttackColor);
                EnemyHealth?.InflictKnocback(knockbackSource, totalKnockbackReceived, isCrit);
                InflictStatusEffects(EnemyHealth);

            }
        }
    }
}
