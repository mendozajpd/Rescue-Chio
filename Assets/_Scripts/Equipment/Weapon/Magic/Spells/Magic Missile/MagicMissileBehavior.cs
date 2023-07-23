using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicMissileBehavior : MonoBehaviour
{

    [Header("Missile Settings")]
    [SerializeField] private float missileTravelSpeed;
    [SerializeField] private float heightDividend = 4;
    private Vector2 _mousePos;
    private Vector2 _startPos;
    private Vector2 _destinationPos;
    private float _height;
    private float _timePassed;
    private bool _underhand;

    [Header("Destination Offset")]
    [Range(0f, 1f)]
    [SerializeField] private float offsetX;
    [Range(0f, 1f)]
    [SerializeField] private float offsetY;

    // Missile Parts
    private ParticleSystem _sparkles;
    private ParticleSystem.EmissionModule _sparklesEmission;
    private ParticleSystem trail;
    private SpriteRenderer _core;

    [Header("Homing Variables")]
    [SerializeField] private float defaultRotationSpeed;
    [SerializeField] private float defaultMissileSpeed;
    [SerializeField] private float defaultAngleOffset = 90;
    [SerializeField] private float missileSpeedDecreaseOvertime = 0.1f;
    [SerializeField] private float missileRotateSpeedIncreaseOvertime = 20f;
    private AggroZone _aggro;
    private Enemy _target;
    private Rigidbody2D _rb;
    private bool _enemyDetected;
    private Vector2 _targetDirection;
    private float _rotateAmount;

    // Lights
    [SerializeField] private float lightIntensityOnDeath;
    private Light2D light2d;


    //temporary
    [SerializeField] private ParticleSystem deathPrefab;



    public void Init(Vector2 mousePosition, Vector2 startPosition, bool isUnderhand)
    {
        _mousePos = mousePosition;
        _startPos = startPosition;
        _underhand = isUnderhand;
    }
    private void OnEnable()
    {
        _aggro.aggroTrigger += _activateHoming;
    }

    private void OnDisable()
    {
    }

    private void Awake()
    {
        _sparkles = GetComponent<ParticleSystem>();
        _sparklesEmission = _sparkles.emission;
        trail = GetComponentInChildren<ParticleSystem>();
        _core = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _aggro = GetComponentInChildren<AggroZone>();
        light2d = GetComponent<Light2D>();
    }

    void Start()
    {
        _sparklesEmission.rateOverTime = 50;
        _getTrajectory();
        _getHeight(_underhand);
        _rotateTowardsTarget();
        StartCoroutine(despawn(5));
    }

    void Update()
    {
        _despawnMissile();
    }

    private void FixedUpdate()
    {
        _travelToDestination();
        if (_enemyDetected && _core != null)
        {
            _getTargetDirection();
            defaultMissileSpeed -= missileSpeedDecreaseOvertime;
            defaultRotationSpeed += missileRotateSpeedIncreaseOvertime;

            _rotateAmount = Vector3.Cross(_targetDirection, transform.up).z;

            _rb.angularVelocity = -_rotateAmount * defaultRotationSpeed;

            // Avoids target, looks pretty cool as a deflect skill
            //rb.angularVelocity = +rotateAmount * rotationSpeed;

            _rb.velocity = transform.up * (defaultMissileSpeed - Vector2.Distance(transform.position, _target.transform.position));
        }


    }

    private void _getTargetDirection()
    {
        _targetDirection = ((Vector2)_target.transform.position - _rb.position).normalized;
    }

    private void _travelToDestination()
    {
        if (!_enemyDetected)
        {
            if (_timePassed < 1 && _core != null)
            {
                _travelTrajectory();
            }

            if (_timePassed > 1)
            {
                _destroyCore();
                _despawnMissile();
            }

        }
    }

    private void _despawnMissile()
    {
        if (_core == null)
        {
            if (light2d.intensity > 0) light2d.intensity -=  lightIntensityOnDeath * 0.01f;
            if (_sparkles.particleCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void _destroyCore()
    {
        if (_core != null)
        {
            _rb.velocity = Vector2.zero;
            Destroy(_core.gameObject);
            Instantiate(deathPrefab, transform.position, Quaternion.identity);
            _sparklesEmission.rateOverTime = 0;
            light2d.intensity = lightIntensityOnDeath;
        }
    }

    private void _travelTrajectory()
    {
        _rotateTowardsTarget();
        _timePassed += Time.deltaTime + (missileTravelSpeed * 0.001f);
        transform.position = MathParabola.Parabola(_startPos, _destinationPos, _height, _timePassed);
        
    }

    private void _getHeight(bool isUnderhand)
    {
        _height = Vector2.Distance(_startPos, _destinationPos) / (isUnderhand ? heightDividend : -heightDividend);
    }


    private void _getTrajectory()
    {

        _destinationPos = new Vector2(_mousePos.x - Random.Range(0, offsetX), _mousePos.y - Random.Range(0, offsetY));
    }


    #region Homing Variables

    private void _activateHoming()
    {
        _target = _aggro.target;
        _aggro.aggroTrigger -= _activateHoming;
        _aggro.gameObject.SetActive(false);
        _sparklesEmission.rateOverTime = 0;
        _sparklesEmission.rateOverDistance = 1;
        _enemyDetected = true;
    }

    private void _rotateTowardsTarget()
    {
        Vector3 directionToTarget = (Vector3)_destinationPos - transform.position;
        float angleRadians = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float angleDegrees = (angleRadians * Mathf.Rad2Deg) - defaultAngleOffset;

        if (angleDegrees < 0f)
        {
            angleDegrees += 360f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
    }


    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _destroyCore();
    }

    IEnumerator despawn(float secondsUntilDeath)
    {
        yield return new WaitForSeconds(secondsUntilDeath);
        _destroyCore();
        Destroy(gameObject);
    }
}
