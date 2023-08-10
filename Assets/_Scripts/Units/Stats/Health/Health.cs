using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Gauge, IDamageable, IHealable
{
    public delegate void DeathHandler();
    public event DeathHandler hasDied;
    private HealthBar _healthBarPrefab;
    private PopUpTextScript _damagePopUp;
    private Color32 normalAttackColor = Color.white;
    private Color32 critAttackColor = Color.red;

    // DamagePopUpPool 
    private DamagePopUpPool _damagePopUpPool;

    // Invoke the method here 
    public System.Action SpawnDamagePopUp;

    [SerializeField] private bool godMode;
    [SerializeField] private bool knockbackImmune;
    private void Awake()
    {
        //_damagePopUp = Resources.Load<PopUpTextScript>("DamagePopUp");
        _damagePopUpPool = GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<DamagePopUpPool>();
        _spawnHealthBar();
    }

    #region DAMAGE FUNCTIONS
    // Can be used to inflict debuff if there is
    public void DamageCrittable(float damageAmount, bool isCrit, Attack attack)
    {
        //PopUpTextScript spawnDamagePopUp = Instantiate(_damagePopUp, transform.position, Quaternion.identity);
        //spawnDamagePopUp.SetPopUpText(damageAmount.ToString(), isCrit, normalAttackColor, critAttackColor);
        _damagePopUpPool.SpawnDamageText(transform.position, isCrit, damageAmount.ToString(), normalAttackColor, critAttackColor);

        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            attack.OnEnemyDeath(this);
            Destroy(gameObject);
            return;
        }

        CurrentValue -= godMode ? 0 : damageAmount;
    }

    public void Damage(float damageAmount, Attack attack)
    {
        var spawnDamagePopUp = Instantiate(_damagePopUp, transform.position, Quaternion.identity);
        spawnDamagePopUp.SetPopUpText(damageAmount.ToString(), normalAttackColor);

        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            attack.OnEnemyDeath(this);
            Destroy(gameObject);
            return;
        }

        CurrentValue -= damageAmount;
    }
    #endregion

    #region HEAL FUNCTIONS
    public void Heal(float healAmount)
    {
        if (CurrentValue + healAmount > MaxValue)
        {
            CurrentValue = MaxValue;
            return;
        }

        CurrentValue += healAmount;
    }
    #endregion

    #region KNOCKBACK FUNCTIONS
    // poison and burning cant be knockback
    // what if there are attacks that should not inflict knockback?

    // Find the right place to add knockback function and how to implement it
    #endregion

    private void _spawnHealthBar()
    {
        _healthBarPrefab = Resources.Load<HealthBar>("Gauge/HealthBarCanvasPrefab");
        var healthBar = Instantiate(_healthBarPrefab, transform);
        healthBar.AssignHealthGauge(this);
    }

}
