using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayCast : MonoBehaviour
{
    private AstralDeathRayBehavior _deathRay;
    private LineRenderer _line;

    private void Awake()
    {
        _deathRay = GetComponentInParent<AstralDeathRayBehavior>();
        _line = GetComponent<LineRenderer>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        _createRaycast();
    }

    private void _createRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.up , _deathRay.LaserDistance);
        transform.position = _deathRay.transform.position;
        Vector3[] positions = new Vector3[]
        {
            transform.position,
            transform.position + Vector3.up * _deathRay.LaserDistance
        };

        _line.SetPositions(positions);
    }
}
