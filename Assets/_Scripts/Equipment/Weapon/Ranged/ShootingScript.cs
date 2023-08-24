using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingScript : MonoBehaviour
{
    // Gun Related Variables
    public RangedWeapon Pistol;

    // Temporary
    [SerializeField] private float bulletSpeed;
    private BulletScript _bulletPrefab;

    // Camera Variable
    [SerializeField] private Camera mainCamera;


    // Object Pool Variables
    [SerializeField] private bool usePool;
    private ObjectPool<BulletScript> _pool;
    private Transform _poolLocation;

    // Direction Variables
    [SerializeField] private Vector2 bulletDirection;
    [SerializeField] private Vector2 mousePos;

    public Vector2 BulletDirection { get => bulletDirection; set => bulletDirection = value; }

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
        _pool?.Clear();
    }

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Pistol = GetComponentInParent<RangedWeapon>();
        _poolLocation = Pistol.GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<ProjectilesPool>().transform;
        _bulletPrefab = Resources.Load<BulletScript>("Units/Player/Weapons/Ranged/Bullets/Bullet");
    }

    void Start()
    {
        _pool = new ObjectPool<BulletScript>(() =>
        {
            var bullet = Instantiate(_bulletPrefab, _poolLocation);
            bullet.Init(_releaseToPool, this);
            return bullet;
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
            bullet.ShootBullet(transform.position);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet.gameObject);
        }, true, 30, 60);

    }

    void Update()
    {
        getBulletDirection();
    }

    private void _shootBullet()
    {
        _pool.Get();
    }

    private void _releaseToPool(BulletScript bullet)
    {
        _pool?.Release(bullet);
    }

    private void getBulletDirection()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        BulletDirection = mousePos - (Vector2)transform.position;
        BulletDirection = BulletDirection.normalized;
    }

}
