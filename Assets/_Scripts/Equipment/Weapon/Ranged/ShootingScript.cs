using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // Gun Related Variables
    public RangedWeapon Pistol;
    private Transform _bulletSpawnPoint;

    // Temporary
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;


    // Camera Variable
    [SerializeField] private Camera mainCamera;

    private void OnEnable()
    {
        Pistol.shootTrigger += _spawnBullet;
    }

    private void OnDisable()
    {
        Pistol.shootTrigger -= _spawnBullet;
    }

    private void Awake()
    {
        Pistol = GetComponentInParent<RangedWeapon>();
        _bulletSpawnPoint = transform;

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void _spawnBullet()
    {
        Instantiate(bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
    }
}
