using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamSpell : Spell
{
    private AstralBeamBehavior _laser;
    private Vector3 _laserStartPosition;
    private Vector3 _laserEndPosition;

    [Header("Laser Settings")]
    [SerializeField] private bool isExplosive = true;
    [SerializeField] private float laserSize = 1;

    // Length(Time) of Laser
    [SerializeField] private float laserDurationLength = 1;
    [SerializeField] private float laserCooldownLength = 0.5f;

    [Header("Wand Actions Variables")]
    [SerializeField] private bool canSwing = false;
    [SerializeField] private float wandAngle = 90;

    public System.Action castTrigger;

    private void Awake()
    {
        SetSpellVariables();
        SetMagicWeaponActions(canSwing, wandAngle);
        _laser = Resources.Load<AstralBeamBehavior>("Player/Weapons/Magic/Spells/AstralBeam/AstralBeamPrefab");
    }

    void Start()
    {
        
    }

    void Update()
    {
        _getLaserPoints();
    }

    public override void CastSpell()
    {
        //_laserTime = laserTimeLength;
        // Spawn laser
        _castAstralBeam(_laser);
        // Give laser settings to laser
        //castTrigger.Invoke();
    }

    private void _castAstralBeam(AstralBeamBehavior laserPrefab)
    {
        var castAstralBeam = Instantiate(laserPrefab, Vector3.zero, Quaternion.identity);
        castAstralBeam.Init(_laserStartPosition, _laserEndPosition, isExplosive ? 0.15f : laserDurationLength, isExplosive ? 0.1f : laserSize);
        castAstralBeam.gameObject.SetActive(true);
        castAstralBeam.explodeTarget();
    }

    private void _getLaserPoints()
    {
        _laserStartPosition = spellHandler.transform.position;
        _laserEndPosition = wand.MouseWorldPosition;
    }







}
