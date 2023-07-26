using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamSpell : Spell
{
    private LineRenderer laser;

    [Header("Laser Settings")]
    [SerializeField] private bool isExplosive = true;

    // Length(Time) of Laser
    [SerializeField] private float laserTimeLength = 1;
    [SerializeField] private float laserCooldownLength = 0.5f;
    private float _laserTime;
    private float _laserCooldownTime;

    // Wand Actions
    [SerializeField] private bool canSwing = false;
    [SerializeField] private float wandAngle = 90;



    private void Awake()
    {
        SetSpellVariables();
        SetMagicWeaponActions(canSwing, wandAngle);

        laser = GetComponentInChildren<LineRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _laserHandlers();
    }

    public override void CastSpell()
    {
        _laserTime = laserTimeLength;

    }

    private void _laserHandlers()
    {
        _laserTimeHandler();

        _laserWidthHandler(_laserTime) ;
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
            _laserCooldownTime -= Time.deltaTime;
        }

        
        if (_laserTime > 0)
        {
            _laserTime -= Time.deltaTime;
        } else
        {
            _laserTime = 0;
        }
    }

}
