using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private ShootingScript muzzle;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private RangedWeapon pistol;
    private Rigidbody2D _rb;

    // Checks
    [SerializeField] private bool hasAlreadyInstantiated;
    // Spawner stuff
    private System.Action<BulletScript> _sendToPool;

    public void Init(System.Action<BulletScript> releaseToPool, ShootingScript bulletSpawner)
    {
        _sendToPool = releaseToPool;
        muzzle = bulletSpawner;
    }

    private void OnEnable()
    {
        if (hasAlreadyInstantiated)
        {
            transform.position = muzzle.transform.position;
            _rb.velocity = new Vector2(muzzle.BulletDirection.x, muzzle.BulletDirection.y).normalized * speed;
            StartCoroutine(despawnSelf(1));
        }
    }

    private void OnDisable()
    {
        _rb.velocity = Vector2.zero;

        if (muzzle == null) Destroy(gameObject);
    }

    private void Awake()
    {
        Debug.Log("Bullet spawned:" + transform.position);
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.position = muzzle.transform.position;
        _rb.velocity = new Vector2(muzzle.BulletDirection.x, muzzle.BulletDirection.y).normalized * speed;
        StartCoroutine(despawnSelf(1));
        hasAlreadyInstantiated = true;
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.tag != "Player") 
        _sendToPool(this);

    }

    IEnumerator despawnSelf(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _sendToPool(this);
    }


}
