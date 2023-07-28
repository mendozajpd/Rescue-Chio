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



    private void OnDestroy()
    {
        _pool.Clear();
    }
    private void Awake()
    {
        _magicMissile = Resources.Load<MagicMissileBehavior>("Player/Weapons/Magic/Spells/MagicMissile/MagicMissilePrefab");
        SetSpellVariables();
        _missileSpawnLocation = spellHandler;
        SetMagicWeaponActions(canSwing, wandAngle);
    }

    private void Start()
    {
        _pool = new ObjectPool<MagicMissileBehavior>(() =>
        {
            var currentSpawnLocation = _missileSpawnLocation.transform.position;
            var magicMissileSpell = Instantiate(_magicMissile, currentSpawnLocation, Quaternion.identity);
            magicMissileSpell.SetSpellSettings(missileTravelSpeed, heightDividend, offsetX, offsetY, homingRotationSpeed, homingMissileSpeed, angleOfObject, missileSpeedDecreaseOvertime, missileRotateSpeedIncreaseOvertime, lightIntensityOnDeath);
            magicMissileSpell.Init(_releaseToPool, wand.MouseWorldPosition, currentSpawnLocation, _determineTrajectorySide());
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
                _isOverhand = !wand.isLookingLeft;
                break;
            case -1:
                _isOverhand = wand.isLookingLeft;
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
        castMagicMissile.Init(_releaseToPool,wand.MouseWorldPosition, currentSpawnLocation, _determineTrajectorySide());
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
