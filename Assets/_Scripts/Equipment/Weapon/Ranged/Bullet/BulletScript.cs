using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletScript : AllyProjectile
{
    [SerializeField] private ShootingScript muzzle;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    private RangedWeapon _gun;
    private Collider2D _hitbox;
    private Rigidbody2D _rb;
    private StatsManager attackerStats;

    // Checks
    [SerializeField] private bool hasAlreadyInstantiated;
    // Spawner stuff
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
    }


    public void ShootBullet(Vector2 bulletStartPos)
    {
        _hitbox.enabled = true;
        transform.position = bulletStartPos;
        attackerStats = muzzle.Pistol.equipment.playerStats;
        _rb.velocity = new Vector2(muzzle.BulletDirection.x, muzzle.BulletDirection.y).normalized * speed;
        StartCoroutine(despawnSelf(1));
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hitbox.enabled = false;
        _rb.velocity = Vector2.zero;
        CollisionDamageKnocbackEnemy(collision, attackerStats, _gun.transform.position, 0, true);
        _sendToPool(this);
    }

    IEnumerator despawnSelf(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _rb.velocity = Vector2.zero;
        _sendToPool(this);
    }


}
