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
    private ObjectPool<BulletScript> _pool;
    [SerializeField] private bool usePool;

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
            bullet.Init(_ReleaseToPool, this);
            return bullet;
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
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
        objectthing.Init(_ReleaseToPool, this);
    }

    private void _shootBullet()
    {
        _spawnBullet(bulletPrefab);
    }

    private void _ReleaseToPool(BulletScript bullet)
    {
        if (usePool)
        {
            _pool.Release(bullet);
        } else
        {
           Destroy(bullet.gameObject);
        }
    }

    private void getBulletDirection()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bulletDirection = mousePos - transform.position;
    }

}
