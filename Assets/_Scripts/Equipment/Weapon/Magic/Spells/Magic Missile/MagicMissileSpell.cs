using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MagicMissileSpell : Spell
{
    private MagicMissileBehavior _magicMissile;
    private SpellHandler _missileSpawnLocation;
    private bool _isOverhand;


    [Header("Spell Settings")]
    [SerializeField] private float defaultSpellDamage;
    [SerializeField] private float defaultSpellKnockback;
    [SerializeField] private float defaultSpellCastSpeed;
    [SerializeField] private float defaultManaCost;
    [SerializeField] private float missileTravelSpeed = 30;
    [SerializeField] private float heightDividend = 8;

    [Header("Accuracy Offset")]
    [Range(0f, 5f)]
    [SerializeField] private float offsetX = 1;
    [Range(0f, 5f)]
    [SerializeField] private float offsetY = 1;

    [Header("Homing Variables")]
    [SerializeField] private float homingRotationSpeed = 800;
    [SerializeField] private float homingMissileSpeed = 45;
    [SerializeField] private float angleOfObject = 90;
    [SerializeField] private float missileSpeedDecreaseOvertime = 0.1f;
    [SerializeField] private float missileRotateSpeedIncreaseOvertime = 50f;

    [Header("Light Variables")]
    [SerializeField] private float lightIntensityOnDeath = 10;

    // Object Pool Variables
    [SerializeField] private bool usePool;
    private ObjectPool<MagicMissileBehavior> _pool;

    // Wand Actions
    [SerializeField] private bool canSwing = true;
    [SerializeField] private float wandAngle = 90;
    [SerializeField] private bool canRotate = true;



    private void OnDestroy()
    {
        _pool?.Clear();
    }


    private void Awake()
    {
        _magicMissile = Resources.Load<MagicMissileBehavior>("Units/Player/Weapons/Magic/Spells/MagicMissile/MagicMissilePrefab");
        SetSpellVariables(defaultSpellDamage, defaultSpellKnockback, defaultSpellCastSpeed, defaultManaCost);
        _missileSpawnLocation = spellHandler;
        SetMagicWeaponActions(canSwing, wandAngle, canRotate);
    }

    private void Start()
    {
        _pool = new ObjectPool<MagicMissileBehavior>(() =>
        {
            Transform poolLocation = GetComponentInParent<UnitsManager>().ObjectPools.GetComponentInChildren<ProjectilesPool>().transform;
            var magicMissileSpell = Instantiate(_magicMissile, poolLocation);
            var currentSpawnLocation = _missileSpawnLocation.transform.position;
            magicMissileSpell.SetSpellSettings(missileTravelSpeed, heightDividend, offsetX, offsetY, homingRotationSpeed, homingMissileSpeed, angleOfObject, missileSpeedDecreaseOvertime, missileRotateSpeedIncreaseOvertime, lightIntensityOnDeath);
            magicMissileSpell.Init(_releaseToPool, wand.MouseWorldPosition, currentSpawnLocation, _determineTrajectorySide(), this);
            return magicMissileSpell;
        }, magicMissileSpell =>
        {
            var currentSpawnLocation = _missileSpawnLocation.transform.position;
            magicMissileSpell.resetSpell(wand.MouseWorldPosition,currentSpawnLocation,_determineTrajectorySide());
            magicMissileSpell.SetSpellSettings(missileTravelSpeed, heightDividend, offsetX, offsetY, homingRotationSpeed, homingMissileSpeed, angleOfObject, missileSpeedDecreaseOvertime, missileRotateSpeedIncreaseOvertime, lightIntensityOnDeath);
            magicMissileSpell.gameObject.SetActive(true);

        }, magicMissileSpell =>
        {
            magicMissileSpell.gameObject.SetActive(false);
        }, magicMissileSpell =>
        {
            Destroy(magicMissileSpell.gameObject);
        }, false, 400, 500);

    }

    private bool _determineTrajectorySide()
    {
        switch (wand.Swing)
        {
            case 1:
                _isOverhand = !wand.IsLookingLeft;
                break;
            case -1:
                _isOverhand = wand.IsLookingLeft;
                break;
        }
        return _isOverhand;
    }

    #region Cast Functions
    public override void CastSpell()
    {
        if (usePool)
        {
            _pool.Get();
            return;
        }
        _castMagicMissile(_magicMissile);
    }

    private void _castMagicMissile(MagicMissileBehavior missilePrefab)
    {
        var currentSpawnLocation = _missileSpawnLocation.transform.position;
        var castMagicMissile = Instantiate(missilePrefab, currentSpawnLocation, Quaternion.identity);
        castMagicMissile.SetSpellSettings(missileTravelSpeed, heightDividend, offsetX, offsetY, homingRotationSpeed, homingMissileSpeed, angleOfObject, missileSpeedDecreaseOvertime, missileRotateSpeedIncreaseOvertime, lightIntensityOnDeath);
        castMagicMissile.Init(_releaseToPool,wand.MouseWorldPosition, currentSpawnLocation, _determineTrajectorySide(), this);
    }
    #endregion

    #region Object Pool Functions
    private void _releaseToPool (MagicMissileBehavior magicSpell)
    {
        if (usePool)
        {
            _pool.Release(magicSpell);
            return;
        }
        Destroy(magicSpell.gameObject);

    }

    #endregion
}
