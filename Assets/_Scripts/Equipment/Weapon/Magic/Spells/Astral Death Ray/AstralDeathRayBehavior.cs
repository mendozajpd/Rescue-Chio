using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer _laser;
    private AstralDeathRaySpell _spell;

    private Rigidbody2D _rb;
    private float _rotationSpeed;
    private float _rotateAmount;
    private float _laserDistance;

    private List<AstralDeathRayParticles> _particles;
    private Light2D _light2D;
    private float _fadeMultiplier = 7;

    public float LaserDistance { get => _laserDistance; }

    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _particles = new List<AstralDeathRayParticles>(GetComponentsInChildren<AstralDeathRayParticles>());
        _light2D = GetComponent<Light2D>();
    }
    public void SetLaserSettings(float laserdistance, float rotationSpeed)
    {
        _laserDistance = laserdistance;
        _rotationSpeed = rotationSpeed;
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
        RaycastHit2D hit = Physics2D.Raycast(_spell.transform.position, transform.position + Vector3.up, _laserDistance);

        if (hit)
        {
            Debug.Log(hit.collider.name);
            Debug.DrawLine(_spell.transform.position, transform.position + Vector3.up, Color.red);
        }

        // minus the y position to the distance between to what was hit

        Vector3[] positions = new Vector3[]
        {
            Vector3.zero,
            hit ? hit.point : (Vector3.up * _laserDistance)
        };

        _laser.SetPositions(positions);
        _rotateLaser(direction);
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

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }


    // FOR COLLIDERS, JUST ADD A COLLIDER AND THEN CHANGE ITS SIZE AND WIDTH TO THE LASER.

}
