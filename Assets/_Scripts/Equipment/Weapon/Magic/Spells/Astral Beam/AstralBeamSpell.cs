using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBeamSpell : Spell
{
    [Header("Spell Settings")]
    [SerializeField] private float defaultSpellDamage;
    [SerializeField] private float defaultSpellKnockback;

    private AstralBeamBehavior _laser;
    private Vector3 _laserStartPosition;
    private Vector3 _laserEndPosition;

    [Header("Laser Settings")]
    [SerializeField] private bool isExplosive = true;
    [SerializeField] private float laserSize = 1;
    [SerializeField] private float defaultExplosiveLaserDurationLength;
    [SerializeField] private float defaultExplosiveLaserSize;

    // Length(Time) of Laser
    [SerializeField] private float laserDurationLength = 1;
    [SerializeField] private float laserCooldownLength = 0.5f;

    [Header("Wand Actions Variables")]
    [SerializeField] private bool canSwing = false;
    [SerializeField] private float wandAngle = 90;
    [SerializeField] private bool canRotate = true;

    public System.Action castTrigger;

    private void Awake()
    {
        SetSpellVariables(defaultSpellDamage, defaultSpellKnockback);
        SetMagicWeaponActions(canSwing, wandAngle, canRotate);
        _laser = Resources.Load<AstralBeamBehavior>("Units/Player/Weapons/Magic/Spells/AstralBeam/AstralBeamPrefab");
    }

    void Start()
    {
        
    }

    void Update()
    {
    }

    public override void CastSpell()
    {
        _castAstralBeam(_laser);
    }

    private void _castAstralBeam(AstralBeamBehavior laserPrefab)
    {
        _getLaserPoints();
        var castAstralBeam = Instantiate(laserPrefab, Vector3.zero, Quaternion.identity);
        castAstralBeam.Init(_laserStartPosition, _laserEndPosition, defaultExplosiveLaserDurationLength, defaultExplosiveLaserSize, this);
        castAstralBeam.gameObject.SetActive(true);
    }

    private void _getLaserPoints()
    {
        _laserStartPosition = transform.position;
        _laserEndPosition = wand.MouseWorldPosition;
    }







}
