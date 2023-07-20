using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileScript : MonoBehaviour
{

    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 destinationPos;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float height;
    [SerializeField] private float heightDividend = 4;
    [SerializeField] private float timePassed;
    [SerializeField] private bool underhand;

    [SerializeField] private Vector2 mousePos;


    [Header("Destination Offset")]
    [Range(0f,1f)]
    [SerializeField] private float offsetX;
    [Range(0f,1f)]
    [SerializeField] private float offsetY;

    [Header("Magic Missile Parts")]    
    // Missile Parts
    [SerializeField] private ParticleSystem sparkles;
    [SerializeField] private ParticleSystem.EmissionModule sparklesEmission;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private SpriteRenderer core;

    [Header("Raycast Variables")]
    private Ray2D _ray;
    private RaycastHit2D _raycastHit;
    private ContactFilter2D _contactFilter2D;

    public void Init(Vector2 mousePosition, Vector2 startPosition, bool isUnderhand)
    {
        mousePos = mousePosition;
        startPos = startPosition;
        underhand = isUnderhand;
    }
    private void Awake()
    {
        sparkles = GetComponent<ParticleSystem>();
        sparklesEmission = sparkles.emission;
        trail = GetComponentInChildren<ParticleSystem>();
        core = GetComponentInChildren<SpriteRenderer>();

        _createContactFilter();
    }

    void Start()
    {
        getTrajectory();
        getHeight(underhand);
    }

    void Update()
    {
        if (timePassed < 1)
        {
            _travelTrajectory();
        }

        if (timePassed > 1)
        {
            despawnMissile();
        }
    }

    private void despawnMissile()
    {
        if (core != null)
        {
            Destroy(core.gameObject);
            sparklesEmission.rateOverTime = 0;
        }

        if (sparkles.particleCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void _travelTrajectory()
    {
        timePassed += Time.deltaTime + speedMultiplier;
        transform.position = MathParabola.Parabola(startPos, destinationPos, height, timePassed);
    }

    private void getHeight(bool isUnderhand)
    {
        height = Vector2.Distance(startPos, destinationPos) / (isUnderhand ? heightDividend : -heightDividend);
    }


    private void getTrajectory()
    {

        destinationPos = new Vector2(mousePos.x - Random.Range(0,offsetX), mousePos.y - Random.Range(0,offsetY));
    }


    #region Homing Variables

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            _ray = new Ray2D(transform.position, enemy.transform.position);
            _raycastHit = Physics2D.Raycast(transform.position, enemy.transform.position);
            if (_raycastHit.collider)
            {
                Debug.DrawRay(transform.position, _raycastHit.point, Color.white);
                Debug.Log("enemy in vicinity");
            } else
            {
                Debug.Log("vision obstructed");
            }
        }
    }


    private void _checkForObstruction()
    {

    }

    private void _createContactFilter()
    {
        _contactFilter2D = new ContactFilter2D();
        _contactFilter2D.useTriggers = false;
        _contactFilter2D.useLayerMask = false;
    }

    #endregion
}
