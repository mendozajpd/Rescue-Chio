using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private ShootingScript muzzle;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private RangedWeapon pistol;
    [SerializeField] private Camera mainCamera;
    private Rigidbody2D _rb;

    private void Awake()
    {
        Debug.Log("Bullet spawned:" + transform.position);
        _rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        getBulletDirection();
    }

    void Start()
    {
        _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        StartCoroutine(despawnSelf(3));
    }

    void Update()
    {
    }


    IEnumerator despawnSelf(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    private void getBulletDirection()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
    }
}
