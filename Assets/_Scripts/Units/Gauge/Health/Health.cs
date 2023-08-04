using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Gauge, IDamageable, IHealable
{
    public delegate void DeathHandler();
    public event DeathHandler hasDied;

    private HealthBar _healthBarPrefab;

    private void Awake()
    {
        _spawnHealthBar();
    }

    public void Damage(float damageAmount)
    {
        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            Destroy(gameObject);
            return;
        }

        CurrentValue -= damageAmount;
    }

    public void Heal(float healAmount)
    {
        if (CurrentValue + healAmount > MaxValue)
        {
            CurrentValue = MaxValue;
            return;
        }

        CurrentValue += healAmount;
    }

    private void _spawnHealthBar()
    {
        _healthBarPrefab = Resources.Load<HealthBar>("Gauge/HealthBarCanvasPrefab");
        var healthBar = Instantiate(_healthBarPrefab, transform);
        healthBar.AssignHealthGauge(this);
    }
}
