using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : Gauge, IDamageable, IHealable
{

    public delegate void DeathHandler();
    public event DeathHandler hasDied;
    private HealthBar _healthBarPrefab;
    private Color32 normalAttackColor = Color.white;
    private Color32 critAttackColor = Color.red;
    private DamagePopUpPool _damagePopUpPool;

    [Header("Knockback Variables")]
    private Rigidbody2D _rb;
    public bool Knockbacked;
    private float _knockbackDuration = 0.25f;
    private float _knockbackTime;

    [Header("Invincibility Variables")]
    public bool Invincible;
    private float _invincibilityDuration = 0.3f; 
    private float _invincibilityTime;

    [SerializeField] private bool godMode;
    [SerializeField] private bool knockbackImmune;

    public float InvincibilityTime 
    { 
        get => _invincibilityTime;
        set 
        { 
            _invincibilityTime = value;
            Invincible = _invincibilityTime <= 0 ? false : true;
        } 
    }

    public float KnockbackTime 
    { 
        get => _knockbackTime;
        set 
        { 
            _knockbackTime = value;
            Knockbacked = _knockbackTime <= 0 ? false : true;
            if (!Knockbacked) 
            { 
                _rb.velocity = Vector2.zero;
                _rb.isKinematic = true;
            }
        } 
    }

    private void Awake()
    {
        _damagePopUpPool = GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<DamagePopUpPool>();
        _rb = GetComponent<Rigidbody2D>();
        _spawnHealthBar();
    }

    private void Update()
    {
        _invincibilityTimer();
        _knockbackTimer();
    }

    private void FixedUpdate()
    {
        KnockbackRecoveryHandler();
    }

    #region Timers
    private void _knockbackTimer()
    {
        if (KnockbackTime > 0)
        {
            KnockbackTime -= Time.deltaTime;

        }
    }

    private void _invincibilityTimer()
    {
        if (InvincibilityTime > 0)
        {
            InvincibilityTime -= Time.deltaTime;
        }
    } 
    #endregion

    #region DAMAGE/INVINCIBILITY FUNCTIONS
    // Can be used to inflict debuff if there is
    public void Damage(float damageAmount, bool isCrit, float invincibilityTime, Attack attack)
    {
        _damagePopUpPool.SpawnDamageText(transform.position, isCrit, damageAmount.ToString(), normalAttackColor, critAttackColor);

        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            attack.OnEnemyDeath(this);
            Destroy(gameObject);
            return;
        }

        CurrentValue -= godMode ? 0 : damageAmount;
        _SetInvincibilityTime(damageAmount, invincibilityTime);
    }

    private void _SetInvincibilityTime(float damageAmount, float iTime)
    {
        _invincibilityTime = (damageAmount == 1 ? _invincibilityDuration / 1.3f : _invincibilityDuration) + iTime;
    }


    //public void Damage(float damageAmount, Attack attack)
    //{
    //    var spawnDamagePopUp = Instantiate(_damagePopUp, transform.position, Quaternion.identity);
    //    spawnDamagePopUp.SetPopUpText(damageAmount.ToString(), normalAttackColor);

    //    if (CurrentValue - damageAmount <= 0)
    //    {
    //        hasDied?.Invoke();
    //        attack.OnEnemyDeath(this);
    //        Destroy(gameObject);
    //        return;
    //    }

    //    CurrentValue -= damageAmount;
    //}

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

    public void InflictKnocback(Vector2 knockbackSource, float knockbackAmt, bool crit)
    {
        _rb.velocity = _rb.velocity / 2;
        _setKnockbackTime();
        _rb.isKinematic = false;
        Knockbacked = true;
        Vector2 direction = (Vector2)transform.position - knockbackSource;
        _rb.AddForce(direction.normalized * (crit ? knockbackAmt * 1.4f : knockbackAmt), ForceMode2D.Impulse);
        // TotalKnockback + extraknockback
    }

    private void _setKnockbackTime()
    {
        KnockbackTime = _knockbackDuration;
    }

    private void KnockbackRecoveryHandler()
    {
        if (Knockbacked)
        {
            Vector2 halfedVelocity = (_rb.velocity * (1 + (KnockbackTime/_knockbackDuration)) / 1.75f);
            _rb.velocity = halfedVelocity;
        }
    }
    #endregion

    private void _spawnHealthBar()
    {
        _healthBarPrefab = Resources.Load<HealthBar>("Gauge/HealthBarCanvasPrefab");
        var healthBar = Instantiate(_healthBarPrefab, transform);
        healthBar.AssignHealthGauge(this);
    }


}
