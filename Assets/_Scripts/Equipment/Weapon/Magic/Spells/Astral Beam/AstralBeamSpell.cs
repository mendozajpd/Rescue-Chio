using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamSpell : Spell
{
    // Length(Time) of Laser
    [SerializeField] private float laserTimeLength;
    private float _laserTime;

    // Wand Actions
    [SerializeField] private bool canSwing = false;


    private void Awake()
    {
        SetSpellVariables();
        SetWandActions(canSwing);
    }

    void Start()
    {
        
    }

    void Update()
    {
        _laserHandler();
    }

    public override void CastSpell()
    {
        
    }

    private void _laserHandler()
    {
        _laserCountdownTimer();

        // if laser countdown timer is less then 1 then change the width until it reaches 0
    }

    private void _laserCountdownTimer()
    {
        if (_laserTime > 0)
        {
            _laserTime -= Time.deltaTime;
        }
    }

}
