using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Gauge, IDamageable, IHealable
{
    public delegate void DeathHandler();
    public event DeathHandler hasDied;

    private HealthBar _healthBarPrefab;

    private PopUpTextScript _damagePopUp;

    private void Awake()
    {
        _damagePopUp = Resources.Load<PopUpTextScript>("DamagePopUp");
        _spawnHealthBar();
    }

    // Can be used to inflict debuff if there is
    public void Damage(float damageAmount, Attack attack)
    {
        var spawnDamagePopUp = Instantiate(_damagePopUp, transform.position, Quaternion.identity);
        spawnDamagePopUp.SetPopUpText(damageAmount.ToString());

        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            attack.OnEnemyDeath(this);
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
