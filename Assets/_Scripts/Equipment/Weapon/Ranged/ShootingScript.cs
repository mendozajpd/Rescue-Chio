using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingScript : MonoBehaviour
{
    public RangedWeapon Pistol;
    private BulletScript _bulletPrefab;
    private Camera mainCamera;
    private ObjectPool<BulletScript> _pool;
    private Transform _poolLocation;
    private Vector2 _bulletDirection;
    private Vector2 _mousePos;

    [SerializeField] private float accuracyOffset = 0.5f;

    public Vector2 BulletDirection { get => _bulletDirection; set => _bulletDirection = value; }

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
        }, false, 30, 60);

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
        _pool.Release(bullet);
    }

    private void getBulletDirection()
    {
        _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        BulletDirection = _mousePos - (Vector2)transform.position;
        calculateAccuracy(Pistol.Accuracy);

        BulletDirection = BulletDirection.normalized;
    }

    private void calculateAccuracy(float accuracyStat)
    {
        float accuracyValue = 10 - accuracyStat;
        float offsetValue = (accuracyValue / 10) * (accuracyOffset * Vector2.Distance(_mousePos,transform.position)); // 3 being the max offset
        float randomX = Random.Range(-offsetValue, offsetValue);
        float randomY = Random.Range(-offsetValue, offsetValue);
        Vector2 calculatedAccuracy = new Vector2(BulletDirection.x + randomX, BulletDirection.y + randomY);
        BulletDirection = calculatedAccuracy;
    }

}
