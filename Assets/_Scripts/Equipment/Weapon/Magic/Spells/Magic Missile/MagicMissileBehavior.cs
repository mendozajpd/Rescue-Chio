using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicMissileBehavior : MonoBehaviour
{

    [Header("Missile Settings")]
    private float _missileTravelSpeed;
    private float _heightDividend;
    private Vector2 _mousePos;
    private Vector2 _startPos;
    private Vector2 _destinationPos;
    private float _height;
    private float _timePassed;
    private bool _underhand;
    private bool _destinationReached = false;

    [Header("Destination Offset")]
    [Range(0f, 5f)]
    private float _offsetX;
    [Range(0f, 5f)]
    private float _offsetY;

    // Missile Parts
    private ParticleSystem _sparkles;
    private ParticleSystem.EmissionModule _sparklesEmission;
    private ParticleSystem trail;
    private SpriteRenderer _core;

    [Header("Homing Variables")]
    private float _defaultRotationSpeed;
    private float _defaultMissileSpeed;
    private float _defaultAngleOffset;
    private float _missileSpeedDecreaseOvertime;
    private float _missileRotateSpeedIncreaseOvertime;
    private AggroZone _aggro;
    private Enemy _target;
    private Rigidbody2D _rb;
    private bool _enemyDetected;
    private Vector2 _targetDirection;
    private float _rotateAmount;

    // Lights
    private float _defaultLightIntensity = 0.8f;
    private float _lightIntensityOnDeath;
    private Light2D _light2d;

    // Particles
    private float _defaultEmissionRate = 50;

    // Death Particles
    private MagicMissileDeathParticles _deathParticles;
    private ParticleSystem _deathExplosion;
    private ParticleSystem.EmissionModule _deathEmission;

    // Destroy Game Object Timer Variables
    [SerializeField] private float timeBeforeDestroy;
    private float _countdownTime;
    private bool _timerActive;


    // Object Pool
    private System.Action<MagicMissileBehavior> _sendToPool;
    [SerializeField] private bool debug_UsingCoroutine;


    public bool DestinationReached 
    { 
        get => _destinationReached; 
        set
        {
            _destinationReached = value;
            if (_destinationReached)
            {
                _destroyCore();
            }
        }
    
    }

    public void Init(System.Action<MagicMissileBehavior> releaseToPool, Vector2 mousePosition, Vector2 startPosition, bool isUnderhand)
    {
        _mousePos = mousePosition;
        _startPos = startPosition;
        _underhand = isUnderhand;
        _sendToPool = releaseToPool;
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
        _light2d = GetComponent<Light2D>();
        _deathParticles = GetComponentInChildren<MagicMissileDeathParticles>();
        _deathExplosion = _deathParticles.GetComponent<ParticleSystem>();
        _deathEmission = _deathExplosion.emission;
    }

    void Start()
    {
        _sparklesEmission.rateOverTime = _defaultEmissionRate;
        _getTrajectory();
        _getHeight(_underhand);
    }

    void Update()
    {
        _lightFadeOutHandler();
        _destroyGameObjectTimer();
    }

    private void FixedUpdate()
    {
        _travelToDestination();
        if (_enemyDetected && _core.gameObject.activeSelf)
        {
            _getTargetDirection();
            _defaultMissileSpeed -= _missileSpeedDecreaseOvertime;
            _defaultRotationSpeed += _missileRotateSpeedIncreaseOvertime;

            _rotateAmount = Vector3.Cross(_targetDirection, transform.up).z;

            _rb.angularVelocity = -_rotateAmount * _defaultRotationSpeed;

            _rb.velocity = transform.up * (_defaultMissileSpeed - Vector2.Distance(transform.position, _target.transform.position));
        }
    }



    #region Despawn Missile Functions

    private void _lightFadeOutHandler()
    {
        if (!_core.gameObject.activeSelf)
        {
            if (_light2d.intensity > 0) _light2d.intensity -= _lightIntensityOnDeath * 0.01f;

        }
    }

    private void _destroyCore()
    {
        if (_core.gameObject.activeSelf)
        {
            _rb.velocity = Vector2.zero;
            _core.gameObject.SetActive(false);
            _sparklesEmission.rateOverTime = 0;
            _light2d.intensity = _lightIntensityOnDeath;
            _playDeathParticles();


            if (debug_UsingCoroutine)
            {
                StartCoroutine(_sendSpellToPool(1));
                return;
            }
            _activateTimer(timeBeforeDestroy);
        }
    }

    private void _playDeathParticles()
    {
        _deathEmission.enabled = true;
        _deathExplosion.Play();
    }

    #region Destroy Timer Functions

    private void _activateTimer(float time)
    {
        _countdownTime = time;
        _timerActive = true;
    }

    private void _destroyGameObjectTimer()
    {
        if (_timerActive)
        {
            if (_countdownTime > 0)
            {
                _countdownTime -= Time.deltaTime;
            }
            else
            {
                _deathEmission.enabled = false;
                _sendToPool(this);
                _timerActive = false;
            }
        }
    }

    #endregion

    #endregion

    #region Trajectory Functions
    private void _getTargetDirection()
    {
        _targetDirection = ((Vector2)_target.transform.position - _rb.position).normalized;
    }

    private void _travelToDestination()
    {
        if (!_enemyDetected)
        {
            if (_timePassed < 1 && _core.gameObject.activeSelf)
            {
                _travelTrajectory();
            }

            // Destination Reached
            if (_timePassed > 1)
            {
                DestinationReached = true;
            }

        }
    }

    private void _travelTrajectory()
    {
        _rotateTowardsTarget();
        _timePassed += Time.deltaTime + (_missileTravelSpeed * 0.001f);
        transform.position = MathParabola.Parabola(_startPos, _destinationPos, _height, _timePassed);

    }

    private void _getHeight(bool isUnderhand)
    {
        _height = Vector2.Distance(_startPos, _destinationPos) / (isUnderhand ? _heightDividend : -_heightDividend);
    }


    private void _getTrajectory()
    {

        _destinationPos = new Vector2(_mousePos.x - Random.Range(0, _offsetX), _mousePos.y - Random.Range(0, _offsetY));
    }

    #endregion

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

    private void _deactivateHoming()
    {
        _target = null;
        _aggro.aggroTrigger += _activateHoming;
        _aggro.gameObject.SetActive(true);
        _sparklesEmission.rateOverTime = _defaultEmissionRate;
        _sparklesEmission.rateOverDistance = 0;
        _enemyDetected = false;
    }

    private void _rotateTowardsTarget()
    {
        Vector3 directionToTarget = (Vector3)_destinationPos - transform.position;
        float angleRadians = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float angleDegrees = (angleRadians * Mathf.Rad2Deg) - _defaultAngleOffset;

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

    IEnumerator _sendSpellToPool(float secondsUntilDeath)
    {
        yield return new WaitForSeconds(secondsUntilDeath);
        _deathEmission.enabled = false;
        _sendToPool(this);
    }

    public void SetSpellSettings(float missileSpeed, float height, float offsx, float offsy, float rotSpeed, float homeSpeed, float angle, float speedDecrease, float rotSpeedIncrease, float lightIntensity)
    {
        _missileTravelSpeed = missileSpeed;
        _heightDividend = height;
        _offsetX = offsx;
        _offsetY = offsy;
        _defaultRotationSpeed = rotSpeed;
        _defaultMissileSpeed = homeSpeed;
        _defaultAngleOffset = angle;
        _missileSpeedDecreaseOvertime = speedDecrease;
        _missileRotateSpeedIncreaseOvertime = rotSpeedIncrease;
        _lightIntensityOnDeath = lightIntensity;
    }

    public void resetSpell(Vector2 targetDestination, Vector2 spawnLocation, bool trajectorySide)
    {
        _deactivateHoming();
        _light2d.intensity = _defaultLightIntensity;
        _mousePos = targetDestination;
        _startPos = spawnLocation;
        _getTrajectory();
        _getHeight(trajectorySide);
        _destinationReached = false;
        _timePassed = 0;
        gameObject.transform.position = spawnLocation;
        _core.gameObject.SetActive(true);

    }
}
