using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletScript : AllyProjectile
{
    [SerializeField] private ShootingScript muzzle;
    private ParticleSystem.EmissionModule _emission;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private int pierceAmount = 1;
    private int enemiesDamaged;
    private RangedWeapon _gun;
    private Collider2D _hitbox;
    private Rigidbody2D _rb;
    private StatsManager attackerStats;

    [SerializeField] private float despawnDuration = 1;
    private float despawnTime;
    private System.Action<BulletScript> _sendToPool;


    public void Init(System.Action<BulletScript> releaseToPool, ShootingScript bulletSpawner)
    {
        _sendToPool = releaseToPool;
        muzzle = bulletSpawner;
        _gun = muzzle.Pistol;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hitbox = GetComponent<Collider2D>();
        _emission = GetComponent<ParticleSystem>().emission;
    }


    public void ShootBullet(Vector2 bulletStartPos)
    {
            enemiesDamaged = 0;
            _hitbox.enabled = true;
            transform.position = bulletStartPos;
            attackerStats = muzzle.Pistol.equipment.playerStats;
            _rb.AddForce(muzzle.BulletDirection * speed, ForceMode2D.Impulse);
            despawnTime = despawnDuration;
            Debug.Log("1");
    }

    void Update()
    {
        _despawnTimer();
    }

    private void _despawnTimer()
    {
        if (despawnTime > 0)
        {
            despawnTime -= Time.deltaTime;
        }

        if (despawnTime <= 0)
        {
            _sendToPool(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pierceAmount != enemiesDamaged)
        {
            CollisionDamageKnocbackEnemy(collision, attackerStats, _gun.transform.position, 0, true);
            Debug.Log("2");
            enemiesDamaged += 1;
        }

        if (pierceAmount == enemiesDamaged)
        {
            if (!_hitbox.enabled) return;
            _sendToPool(this);
            _hitbox.enabled = false;
        }
    }

}
