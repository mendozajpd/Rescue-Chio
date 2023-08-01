using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer _laser;
    private AstralDeathRaySpell _spell;

    private Rigidbody2D _rb;
    private float _rotationSpeed;
    private float _rotateAmount;
    private float _laserDistance;


    public void SetLaserSettings(Vector3 startpos, Vector3 endpos, float laserdistance, float rotationSpeed)
    {
        _laserDistance = laserdistance;
        _rotationSpeed = rotationSpeed;
    }


    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody2D>();
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

    }

    public void ActivateLaser(float width)
    {
        _laser.startWidth = width;
        _laser.endWidth = width;
    }
    
    public void DeactivateLaser()
    {
        _laser.startWidth = 0;
        _laser.endWidth = 0;
    } 


    private void _setLaserPositions()
    {
        int ignoreLayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy"));
        Vector2 direction = _spell.wand.MouseWorldPosition - (Vector2)_spell.transform.position; // Direction to the target
        
        // Get the hit point and the start point, subtract the hit point and the end point
        // Use the remainding for the total length
        RaycastHit2D hit = Physics2D.Raycast(_spell.transform.position, direction.normalized, _laserDistance, ignoreLayer);



        Vector3[] positions = new Vector3[]
        {
            Vector3.zero,
            (Vector3.up * _laserDistance)
        };

        _laser.SetPositions(positions);
        _rotateLaser(direction);
    }

    private void _rotateLaser(Vector2 direction)
    {
        _rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -_rotateAmount * _rotationSpeed;

    }

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }

}
