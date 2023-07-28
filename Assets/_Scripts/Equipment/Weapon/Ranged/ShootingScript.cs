using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingScript : MonoBehaviour
{
    // Gun Related Variables
    public RangedWeapon Pistol;
    private Transform _bulletSpawnPoint;

    // Temporary
    [SerializeField] private BulletScript bulletPrefab;
    [SerializeField] private float bulletSpeed;


    // Camera Variable
    [SerializeField] private Camera mainCamera;


    // Object Pool Variables
    [SerializeField] private bool usePool;
    private ObjectPool<BulletScript> _pool;

    // Direction Variables
    [SerializeField] private Vector3 bulletDirection;
    [SerializeField] private Vector3 mousePos;

    public Vector3 BulletDirection { get => bulletDirection; }


    private void OnEnable()
    {
        Pistol.shootTrigger += _shootBullet;
    }

    private void OnDisable()
    {
        Pistol.shootTrigger -= _shootBullet;
    }

    private void OnDestroy()
    {
        _pool.Clear();
    }

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Pistol = GetComponentInParent<RangedWeapon>();
        _bulletSpawnPoint = transform;

    }
    void Start()
    {
        _pool = new ObjectPool<BulletScript>(() =>
        {
            var bullet = Instantiate(bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            bullet.Init(_releaseToPool, this);
            return bullet;
        }, bullet =>
        {
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
            bullet.ShootBullet();

        }, bullet =>
        {
            bullet.Rb2D.velocity = Vector2.zero;
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet.gameObject);
        }, false, 30, 60);

    }

    void Update()
    {
        getBulletDirection();
    }

    private void _spawnBullet(BulletScript bullet)
    {
        if (usePool)
        {
            _pool.Get();
            return;
        }
        var objectthing = Instantiate(bullet, _bulletSpawnPoint.position, Quaternion.identity);
        objectthing.Init(_releaseToPool, this);
    }

    private void _shootBullet()
    {
        _spawnBullet(bulletPrefab);
    }

    private void _releaseToPool(BulletScript bullet)
    {
        if (usePool)
        {
            _pool.Release(bullet);
            return;
        }
        // changed this 
        Destroy(bullet.gameObject);
    }

    private void getBulletDirection()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bulletDirection = mousePos - transform.position;
    }

}
