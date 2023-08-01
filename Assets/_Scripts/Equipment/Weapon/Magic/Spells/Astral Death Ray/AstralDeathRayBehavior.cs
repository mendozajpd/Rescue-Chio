using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer _laser;
    private AstralDeathRaySpell _spell;

    [Header("Laser Settings")]
    private float _laserDistance;

    [SerializeField] private float laserLengthSpeed;
    private Vector3 _startPoint;
    private Vector3 _endPoint;


    [SerializeField] private Vector2 _MouseWorldPosition;
    [SerializeField] private Vector2 _direction;

    public void SetLaserSettings(Vector3 startpos, Vector3 endpos, float laserdistance)
    {
        _startPoint = startpos;
        _endPoint = endpos;
        _laserDistance = laserdistance;
    }


    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
    }

    void Start()
    {
    }

    void Update()
    {
        LaserHandlers();
    }


    public void LaserHandlers()
    {
        if (!gameObject.activeSelf) return;

        _setLaserPositions(_startPoint, _endPoint);

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


    private void _setLaserPositions(Vector3 startPoint, Vector3 endPoint)
    {
        int ignoreLayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy"));
        Vector2 direction = _spell.wand.MouseWorldPosition - (Vector2)_spell.transform.position; // Direction to the target
        RaycastHit2D hit = Physics2D.Raycast(_spell.transform.position, direction.normalized, _laserDistance, ignoreLayer);


        Vector3[] positions = new Vector3[]
        {
            startPoint,
            hit ? hit.point: _startPoint + ((Vector3)direction.normalized * _laserDistance)
        };

        _laser.SetPositions(positions);
    }

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }

}
