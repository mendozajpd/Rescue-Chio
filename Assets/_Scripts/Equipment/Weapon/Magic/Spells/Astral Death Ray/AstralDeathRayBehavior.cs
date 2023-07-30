using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer laser;

    [Header("Laser Settings")]
    private float _laserSize;

    [SerializeField] private float laserLengthSpeed;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    public void SetLaserSettings(Vector3 startpos, Vector3 endpos, float laserSize)
    {
        _startPoint = startpos;
        _endPoint = endpos;
        _laserSize = laserSize;
    }


    private void Awake()
    {
        laser = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _setLaserPositions(_startPoint, _endPoint);
    }

    void Update()
    {
        _laserHandlers();
    }


    private void _laserHandlers()
    {
        if (!gameObject.activeSelf) return;

        _laserWidthHandler(_laserSize);
        _setLaserPositions(_startPoint, _endPoint);
    }

    private void _laserWidthHandler(float width)
    {
        laser.startWidth = width;
        laser.endWidth = width;

    }


    private void _setLaserPositions(Vector3 startPoint, Vector3 endPoint)
    {

        Vector3[] positions = new Vector3[]
        {
            startPoint,
            endPoint
        };

        laser.SetPositions(positions);
    }

}
