using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamBehavior : MonoBehaviour
{
    private LineRenderer laser;

    [Header("Laser Settings")]
    [SerializeField] private bool isExplosive = true;
    private float _laserSize;

    // Length(Time) of Laser
    private float _laserDurationLength;
    private float _laserDurationTime;
    private float _laserCooldownTime;

    [SerializeField] private float laserLengthSpeed;
    private Vector3 _startPoint;
    private Vector3 _endPoint;


    // Explosion
    private Explosion_Default explosion;

    public void Init(Vector3 startpos, Vector3 endpos, float laserDuration, float laserSize)
    {
        _startPoint = startpos;
        _endPoint = endpos;
        _laserDurationTime = laserDuration;
        _laserDurationLength = laserDuration;
        _laserSize = laserSize;
    }


    private void Awake()
    {
        laser = GetComponent<LineRenderer>();
        explosion = Resources.Load<Explosion_Default>("Explosions/Explosion_Astral");
    }

    void Start()
    {
        _setLaserPositions(_startPoint, _endPoint);
    }

    void Update()
    {
        // should only run when gameobject is active
        _laserHandlers();
    }


    private void _laserHandlers()
    {
        if (!gameObject.activeSelf) return;
            
        _laserTimeHandler();
        _laserWidthHandler(_laserDurationTime);
        // if laser countdown timer is less then 1 then change the width until it reaches 0

    }

    private void _laserWidthHandler(float width)
    {
            laser.startWidth = width;
            laser.endWidth = width;

    }

    private void _laserTimeHandler()
    {
        if (_laserCooldownTime > 0)
        {
            _laserCooldownTime -= (Time.deltaTime + (laserLengthSpeed));
        }


        if (_laserDurationTime > 0)
        {
            _laserDurationTime -= (Time.deltaTime + (laserLengthSpeed));
        }
        else
        {
            _laserDurationTime = 0;

            if (isExplosive) Destroy(gameObject);
        }
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

    public void explodeTarget()
    {
        var explod = Instantiate(explosion, _endPoint, Quaternion.identity);
        explod.Explode();
    }
    
}
