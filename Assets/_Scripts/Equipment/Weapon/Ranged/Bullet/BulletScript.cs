using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private ShootingScript muzzle;

    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private RangedWeapon pistol;
    public Rigidbody2D Rb2D;

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

    }


    private void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    public void ShootBullet(Vector2 bulletStartPos)
    {
        transform.position = bulletStartPos;
        Rb2D.velocity = new Vector2(muzzle.BulletDirection.x, muzzle.BulletDirection.y).normalized * speed;
        StartCoroutine(despawnSelf(1));
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // do bullet things
        _sendToPool(this);
    }

    IEnumerator despawnSelf(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _sendToPool(this);
    }


}
