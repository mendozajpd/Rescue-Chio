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
    [SerializeField] private float angleOffset = 90;
    private Vector2 targetDirection;
    private float rotateAmount;


    //temporary
    [SerializeField] private ParticleSystem deathPrefab;




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
        sparklesEmission.rateOverTime = 50;
        getTrajectory();
        getHeight(underhand);
    }

    void Update()
    {
        _despawnMissile();
    }

    private void FixedUpdate()
    {
        _travelToDestination();
        if (enemyDetected)
        {
            _getTargetDirection();
            missileSpeed -= 0.3f;

            rotateAmount = Vector3.Cross(targetDirection, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotationSpeed;

            rb.velocity = transform.up * (missileSpeed - Vector2.Distance(transform.position, target.transform.position));
        }


    }

    private void _getTargetDirection()
    {
        targetDirection = ((Vector2)target.transform.position - rb.position).normalized;
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
                _despawnMissile();
            }
        }
    }

    private void _despawnMissile()
    {
        if (sparkles.particleCount == 0 && core == null)
        {
            Destroy(gameObject);
        }
    }

    private void _destroyCore()
    {
        if (core != null)
        {
            Destroy(core.gameObject);
            Instantiate(deathPrefab, transform.position, Quaternion.identity);
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
        target = aggro.target;
        aggro.aggroTrigger -= _activateHoming;
        aggro.gameObject.SetActive(false);
        //gameObject.transform.rotation = new Quaternion(0,0,targetDirection.z);
        _rotateClockwiseToTarget();
        sparklesEmission.rateOverTime = 0;
        sparklesEmission.rateOverDistance = 1;
        enemyDetected = true;
    }

    private void _rotateClockwiseToTarget()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float angleRadians = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float angleDegrees = (angleRadians * Mathf.Rad2Deg) - angleOffset;

        if (angleDegrees < 0f)
        {
            angleDegrees += 360f;
        }

        Debug.Log("ROTATED TO " + angleDegrees);

        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
    }


    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _destroyCore();
    }
}
