using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayBehavior : MonoBehaviour
{
    private LineRenderer _laser;
    private AstralDeathRaySpell _spell;

    [Header("Laser Settings")]
    private float _laserSize;
    private float _laserDistance;

    [SerializeField] private float laserLengthSpeed;
    private Vector3 _startPoint;
    private Vector3 _endPoint;


    [SerializeField] private Vector2 _MouseWorldPosition;
    [SerializeField] private Vector2 _direction;

    public void SetLaserSettings(Vector3 startpos, Vector3 endpos, float laserSize, float laserdistance)
    {
        _startPoint = startpos;
        _endPoint = endpos;
        _laserSize = laserSize;
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
        _laserHandlers();
    }


    private void _laserHandlers()
    {
        if (!gameObject.activeSelf) return;

        _laserWidthHandler(_laserSize);
        Vector2 lessY = new Vector2(_spell.wand.IsLookingLeft ? 0.27f : -0.27f, -1.25f);
        Vector2 direction = _spell.wand.MouseWorldPosition - ((Vector2)_spell.wand.PlayerPos - lessY); ;
        _setLaserPositions(_startPoint, direction * _laserDistance, direction);

    }

    private void _laserWidthHandler(float width)
    {
        _laser.startWidth = width;
        _laser.endWidth = width;

    }


    private void _setLaserPositions(Vector3 startPoint, Vector3 endPoint, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(_spell.transform.position, direction.normalized, direction.magnitude);

        Vector3[] positions = new Vector3[]
        {
            startPoint,
            hit ? hit.point: endPoint
        };

        _laser.SetPositions(positions);
    }

    public void _setSpell(AstralDeathRaySpell spell)
    {
        _spell = spell;
    }

}
