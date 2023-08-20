using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamBehavior : MonoBehaviour
{
    private LineRenderer laser;
    private Spell spellSource;

    [Header("Laser Settings")]
    [SerializeField] private bool isExplosive = true;
    private float _laserSize;

    // Length(Time) of Laser
    private float _laserDurationTime;
    private float _laserCooldownTime;

    [SerializeField] private float laserLengthSpeed;
    private Vector3 _startPoint;
    private Vector3 _endPoint;


    // Explosion
    private Explosion_Default explosion;

    public void Init(Vector3 startpos, Vector3 endpos, float laserDuration, float laserSize, Spell spell)
    {
        spellSource = spell;
        _startPoint = startpos;
        _endPoint = endpos;
        _laserDurationTime = laserDuration;
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
        _laserHandlers();
    }


    private void _laserHandlers()
    {
        if (!gameObject.activeSelf) return;
            
        _laserTimeHandler();
        _laserWidthHandler(_laserDurationTime);

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
        float distance = Vector2.Distance(startPoint, endPoint);
        Vector2 direction = endPoint - startPoint;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction.normalized, distance, GetIgnoreLayerMask("Player", "AllyProjectiles"));
        Vector3[] positions = new Vector3[]
        {
            startPoint,
            hit ? hit.point : endPoint
        };

        laser.SetPositions(positions);

        if (hit) 
        { 
            explodeTarget(hit.point);
            return;
        }

        explodeTarget(endPoint);

    }

    public void explodeTarget(Vector2 explosionLocation)
    {
        UnitManager explosionSource = spellSource.wand.equipment.Unit;
        var explode = Instantiate(explosion, explosionLocation, Quaternion.identity);
        explode.SetExplosionSource(explosionSource);
        explode.Explode();
    }

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
}
