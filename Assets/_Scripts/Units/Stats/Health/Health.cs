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
    private TextPopupPool _popupTextPool;

    [Header("Knockback Variables")]
    private Rigidbody2D _rb;
    public bool Knockbacked;
    private float _knockbackDuration = 0.25f;
    private float _knockbackTime;

    [Header("Invincibility Variables")]
    public bool Invincible;
    private float _invincibilityDuration = 0.1f;
    private float _invincibilityTime;

    [SerializeField] private bool godMode;
    [SerializeField] private bool knockbackImmune;


    // TEMPORARY
    [SerializeField] private bool debugMode;
    private float _damagePerSecondTime;
    private float _dps;


    private float _knockbackRecoverySmoothness = 0.835f;
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
        _popupTextPool = GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<TextPopupPool>();
        _rb = GetComponent<Rigidbody2D>();
        _spawnHealthBar();
    }

    private void Update()
    {
        _timerHandler();
    }

    private void FixedUpdate()
    {
        KnockbackRecoveryHandler();
    }

    #region TIMERS
    private void _timerHandler()
    {
        _invincibilityTimer();
        _knockbackTimer();
         if (debugMode) _damageReceivedTimer();
    }

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

    private void _damageReceivedTimer()
    {
        if (_damagePerSecondTime > 0)
        {
            _damagePerSecondTime -= Time.deltaTime;
        }

        if (_damagePerSecondTime <= 0)
        {
            Debug.Log("damge dealt in 1 second: " + _dps);
            _dps = 0;
            _damagePerSecondTime = 1;
        }
    }
    #endregion

    #region DAMAGE/INVINCIBILITY FUNCTIONS
    // Can be used to inflict debuff if there is
    public void Damage(float damageAmount, bool isCrit, float invincibilityTime, Attack attack)
    {
        _popupTextPool.SpawnDamageText(transform.position, isCrit, damageAmount.ToString(), normalAttackColor, critAttackColor);

        if (CurrentValue - damageAmount <= 0)
        {
            hasDied?.Invoke();
            attack.OnEnemyDeath(this);
            Destroy(gameObject);
            return;
        }

        _dps += damageAmount;
        CurrentValue -= godMode ? 0 : damageAmount;
        _SetInvincibilityTime(damageAmount, invincibilityTime);
    }

    private void _SetInvincibilityTime(float damageAmount, float iTime)
    {
        _invincibilityTime = (damageAmount == 1 ? _invincibilityDuration / 1.3f : _invincibilityDuration) + iTime;
    }

    #endregion

    #region HEAL FUNCTIONS
    public void Heal(float healAmount, bool usePopup)
    {
        if (CurrentValue + healAmount > MaxValue)
        {
            CurrentValue = MaxValue;
            return;
        }

        CurrentValue += healAmount;
        if(usePopup) _popupTextPool.SpawnHealText(transform.position, healAmount.ToString());
    }
    #endregion

    #region KNOCKBACK FUNCTIONS
    // poison and burning cant be knockback
    // what if there are attacks that should not inflict knockback?

    public void InflictKnocback(Vector2 knockbackSource, float knockbackAmt, bool crit)
    {
        if (!knockbackImmune)
        {
            if (float.IsNaN(knockbackAmt)) knockbackAmt = 0.1f;
            _rb.velocity = Vector2.zero;
            _setKnockbackTime();
            _rb.isKinematic = false;
            Knockbacked = true;
            Vector2 direction = (Vector2)transform.position - knockbackSource;
            _rb.AddForce(direction.normalized * (crit ? knockbackAmt * 1.4f : knockbackAmt), ForceMode2D.Impulse);
        }

    }

    private void _setKnockbackTime()
    {
        KnockbackTime = _knockbackDuration;
    }

    private void KnockbackRecoveryHandler()
    {
        if (Knockbacked)
        {
            Vector2 newVelocity = _rb.velocity * _knockbackRecoverySmoothness;
            _rb.velocity = newVelocity;
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
