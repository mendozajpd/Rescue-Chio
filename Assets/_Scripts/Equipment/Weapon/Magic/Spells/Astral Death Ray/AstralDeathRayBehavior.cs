using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(EdgeCollider2D))]
public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer _laser;
    private AstralDeathRaySpell _spell;

    private Rigidbody2D _rb;
    private float _rotationSpeed;
    private float _rotateAmount;
    private float _laserDistance;
    private float _laserSize;

    private List<AstralDeathRayParticles> _particles;
    private Light2D _light2D;
    private float _fadeMultiplier = 7;


    private EdgeCollider2D _laserHitbox;

    //Laser Tip
    private AstralDeathRayEnd _tipPrefab;
    public AstralDeathRayEnd LaserTip;

    public float LaserDistance 
    { 
        get => _laserDistance;
        set
        {
            _laserDistance = value;
            Vector3 newPos = new Vector3(0, _laserDistance);
            LaserTip.SetLaserTipPosition(newPos, LaserTip);
            Debug.Log("new position set");
        }
    }


    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _particles = new List<AstralDeathRayParticles>(GetComponentsInChildren<AstralDeathRayParticles>());
        _light2D = GetComponent<Light2D>();
        _laserHitbox = GetComponent<EdgeCollider2D>();
        _tipPrefab = Resources.Load<AstralDeathRayEnd>("Player/Weapons/Magic/Spells/AstralDeathRay/LaserEnd");
        _spawnLaserTip();
    }

    public void SetLaserSettings(float laserdistance, float rotationSpeed, float size)
    {
        LaserDistance = laserdistance;
        _rotationSpeed = rotationSpeed;
        _laserSize = size;
    }

    void Start()
    {
        DeactivateLaser();
    }

    void Update()
    {
        LaserHandlers();

    }

    public void LaserHandlers()
    {
        if (!gameObject.activeSelf) return;
        
        _laserStartPointPositionHandler();
        _setLaserPositions();
        _lightHandler();
    }

    private void _lightHandler()
    {
        if(_spell.CurrentCharge == 100)
        {
            _light2D.intensity = Random.Range(2f, 5f);
            return;
        }

        if (_light2D.intensity > 0 )
        {
            _light2D.intensity -= Time.deltaTime * _fadeMultiplier;
        } else
        {
            _light2D.intensity = 0;
        }
    }

    #region Collision Related Variables
    private void _setEdgeCollider(LineRenderer laser)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < laser.positionCount; point++)
        {
            Vector3 laserPoint = laser.GetPosition(point);
            edges.Add(new Vector2(laserPoint.x, laserPoint.y));
        }

        _laserHitbox.SetPoints(edges);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var Enemy = collision.GetComponent<Enemy>();

        if (Enemy != null)
        {
            Debug.Log("Damaged " + Enemy.name);
        }
    }


    #endregion

    #region Enable/Disable Laser
    public void ActivateLaser(float width)
    {
        _laser.startWidth = width;
        _laser.endWidth = width;
        _laserHitbox.enabled = true;

        foreach (AstralDeathRayParticles particle in _particles)
        {
            particle.EnableParticles();
        }
    }

    public void DeactivateLaser()
    {
        _laser.startWidth = 0;
        _laser.endWidth = 0;
        _laserHitbox.enabled = false;

        foreach (AstralDeathRayParticles particle in _particles)
        {
            particle.DisableParticles();
        }
    }
    #endregion

    #region Laser Position/Rotation
    private void _laserStartPointPositionHandler()
    {
        transform.position = _spell.transform.position;
    }

    private void _setLaserPositions()
    {
        Vector2 mousePosition = _spell.wand.MouseWorldPosition - (Vector2)_spell.transform.position; // Direction to the target
        Vector2 laserDirection = LaserTip.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection.normalized, LaserDistance, GetIgnoreLayerMask("Player", "AllyProjectiles", "Enemy"));
        float distanceBetweenStartEnd = Vector3.Distance(transform.position, hit.point);
        Vector3 newLaserLength = new Vector3(0, distanceBetweenStartEnd, 0);

        Vector3[] positions = new Vector3[]
        {
            Vector3.zero,
           hit ? newLaserLength : (Vector3.up * LaserDistance)
        };

        _laser.SetPositions(positions);
        _setEdgeCollider(_laser);
        _rotateLaser(mousePosition);
    }

    private void _rotateLaser(Vector2 direction)
    {
        _rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -_rotateAmount * _rotationSpeed;

    }

    #endregion

    #region Raycast Functions
    private int GetIgnoreLayerMask(params string[] layerNamesToIgnore)
    {
        int layerMask = 0;

        foreach (string layerName in layerNamesToIgnore)
        {
            int layer = LayerMask.NameToLayer(layerName);
            layerMask |= 1 << layer;
        }

        return ~layerMask;
    }


    #endregion

    #region LaserTip Functions
    private void _spawnLaserTip()
    {
        var laserTip = Instantiate(_tipPrefab, transform);
        laserTip.SetDeathRay(this);
        LaserTip = laserTip;
    }


    #endregion

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }



}
