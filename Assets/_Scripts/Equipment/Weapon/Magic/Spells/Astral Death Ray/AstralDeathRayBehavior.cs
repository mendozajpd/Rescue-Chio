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

    //Collider
    private EdgeCollider2D _laserHitbox;

    // Laser End
    //private AstralDeathRayEnd _tipPrefab;
    //public AstralDeathRayEnd LaserTip;

    public float LaserDistance 
    { 
        get => _laserDistance;
        set
        {
            _laserDistance = value;
            //Vector3 newPos = new Vector3(transform.position.x, _laserDistance);
            //LaserTip.SetLaserTipPosition(newPos, LaserTip);
            //Debug.Log("new position set");
        }
    }


    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _particles = new List<AstralDeathRayParticles>(GetComponentsInChildren<AstralDeathRayParticles>());
        _light2D = GetComponent<Light2D>();
        _laserHitbox = GetComponent<EdgeCollider2D>();
        //_tipPrefab = Resources.Load<AstralDeathRayEnd>("Player/Weapons/Magic/Spells/AstralDeathRay/LaserEnd");
        //_spawnLaserTip();
    }


    // Only do this when laser distance, rotationspeed, and or size has changed
    public void SetLaserSettings(float laserdistance, float rotationSpeed, float size)
    {
        LaserDistance = laserdistance;
        _rotationSpeed = rotationSpeed;
        _laserSize = size;
        //_laserHitbox.size = new Vector2(size, laserdistance);
        //_laserHitbox.offset = new Vector2(0, laserdistance / 2);
    }

    void Start()
    {
    }

    void Update()
    {
        LaserHandlers();

    }

    private void _laserStartPointPositionHandler()
    {
        transform.position = _spell.transform.position;
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

    private void _setEdgeCollider(LineRenderer laser)
    {
        List<Vector2> edges = new List<Vector2>();

        for(int point = 0; point < laser.positionCount; point++)
        {
            Vector3 laserPoint = laser.GetPosition(point);
            edges.Add(new Vector2(laserPoint.x, laserPoint.y));
        }

        _laserHitbox.SetPoints(edges);
    }


    public void ActivateLaser(float width)
    {
        _laser.startWidth = width;
        _laser.endWidth = width;

        foreach (AstralDeathRayParticles particle in _particles)
        {
            particle.EnableParticles();
        }
    }
    
    public void DeactivateLaser()
    {
        _laser.startWidth = 0;
        _laser.endWidth = 0;

        foreach (AstralDeathRayParticles particle in _particles)
        {
            particle.DisableParticles();
        }
    } 

    

    private void _setLaserPositions()
    {
        //int ignoreLayer = GetIgnoreLayerMask();
        Vector2 direction = _spell.wand.MouseWorldPosition - (Vector2)_spell.transform.position; // Direction to the target

        // Get the hit point and the start point, subtract the hit point and the end point
        // Use the remainding for the total length
        //RaycastHit2D hit = Physics2D.Raycast(_spell.transform.position, transform.position + Vector3.up, _laserDistance);

        //if (hit)
        //{
        //    Debug.Log(hit.collider.name);
        //    Debug.DrawLine(_spell.transform.position, transform.position + Vector3.up, Color.red);
        //}

        // minus the y position to the distance between to what was hit

        Vector3[] positions = new Vector3[]
        {
            Vector3.zero,
            _laserHitbox.IsTouchingLayers(LayerMask.NameToLayer("Enemy")) ? new Vector3(0,_laserHitbox.ClosestPoint(_spell.transform.position).magnitude, 0) * 1.3f :(Vector3.up * LaserDistance)
        };

        _laser.SetPositions(positions);
        _setEdgeCollider(_laser);
        _rotateLaser(direction);
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        //ContactFilter2D contact = new ContactFilter2D();
        //Debug.Log("distance between: " + _laserHitbox.GetContacts());
    }

    private void _rotateLaser(Vector2 direction)
    {
        _rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -_rotateAmount * _rotationSpeed;

    }

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

    #region LaserTip Functions
    //private void _spawnLaserTip()
    //{
    //    var laserTip = Instantiate(_tipPrefab, transform);
    //    laserTip.SetDeathRay(this);
    //    LaserTip = laserTip;
    //}


    #endregion

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }


    // FOR COLLIDERS, JUST ADD A COLLIDER AND THEN CHANGE ITS SIZE AND WIDTH TO THE LASER.

}
