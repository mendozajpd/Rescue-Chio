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
    [Range(0f, 1f)]
    [SerializeField] private float offsetX;
    [Range(0f, 1f)]
    [SerializeField] private float offsetY;

    [Header("Magic Missile Parts")]
    // Missile Parts
    [SerializeField] private ParticleSystem sparkles;
    [SerializeField] private ParticleSystem.EmissionModule sparklesEmission;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private SpriteRenderer core;

    [Header("Homing Variables")]
    [SerializeField] private AggroZone aggro;
    [SerializeField] private Enemy target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool enemyDetected;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float missileSpeed;




    public void Init(Vector2 mousePosition, Vector2 startPosition, bool isUnderhand)
    {
        mousePos = mousePosition;
        startPos = startPosition;
        underhand = isUnderhand;
    }
    private void OnEnable()
    {
        aggro.aggroTrigger += _activateHoming;
    }

    private void OnDisable()
    {
        aggro.aggroTrigger -= _activateHoming;
    }

    private void Awake()
    {
        sparkles = GetComponent<ParticleSystem>();
        sparklesEmission = sparkles.emission;
        trail = GetComponentInChildren<ParticleSystem>();
        core = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        aggro = GetComponentInChildren<AggroZone>();
    }

    void Start()
    {
        getTrajectory();
        getHeight(underhand);
    }

    void Update()
    {
        _travelToDestination();
    }

    private void FixedUpdate()
    {
        if (enemyDetected)
        {
            Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotationSpeed;

            rb.velocity = transform.up * missileSpeed;
        }
    }

    private void _travelToDestination()
    {
        if (!enemyDetected)
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

        destinationPos = new Vector2(mousePos.x - Random.Range(0, offsetX), mousePos.y - Random.Range(0, offsetY));
    }


    #region Homing Variables

    private void _activateHoming()
    {
        enemyDetected = true;
        target = aggro.target;
    }


    #endregion
}
