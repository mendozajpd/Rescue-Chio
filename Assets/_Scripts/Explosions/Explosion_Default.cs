using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CircleCollider2D))]
public class Explosion_Default : Attack, IExplode
{
    private UnitManager explosionSource;
    private ParticleSystem _particles;
    private Light2D _light2D;

    private List<UnitManager> damagedByExplosion = new List<UnitManager>();
    [SerializeField] private bool isFriendly;
    [SerializeField] private float intensityFadeSpeed = 1f;
    [SerializeField] private float volumentricIntensityFadeSpeed = 3;
    [SerializeField] private float explosionDuration = 1;
    private float explosionTime;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _light2D = GetComponent<Light2D>();
        explosionTime = explosionDuration;
        inflictsKnockback = true;
    }

    private void Update()
    {
        _explosionTimer();
    }

    private void _explosionTimer()
    {
        if (explosionTime > 0)
        {
            explosionTime -= Time.deltaTime;
            if (explosionTime < explosionDuration * .8f)
            {
                _deactivateExplosionHitbox();
            }
        }

        if (explosionTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_light2D.intensity > 0)
        {
            _light2D.intensity -= intensityFadeSpeed;
        }
        else
        {
            _light2D.intensity = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFriendly)
        {
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                if (!damagedByExplosion.Contains(enemy))
                {
                    var attackerStats = explosionSource.UnitStats;
                    DealDamageToEnemy(collision, attackerStats, this.transform.position, 0);
                    _addUnitToDamaged(enemy);
                }
            }
        }
        else
        {
            // deal damage to everything
        }


    }

    public void Explode()
    {
        _particles.Play();
    }

    public void SetExplosionSource(UnitManager unit)
    {
        explosionSource = unit;
    }

    private void _addUnitToDamaged(UnitManager unit)
    {
        damagedByExplosion.Add(unit);
    }
    
    private void _deactivateExplosionHitbox()
    {
        Collider2D hitbox = GetComponent<Collider2D>();
        if (hitbox.enabled) hitbox.enabled = false;
    }

}
