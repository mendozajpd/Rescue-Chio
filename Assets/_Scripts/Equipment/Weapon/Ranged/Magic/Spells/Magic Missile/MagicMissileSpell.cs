using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileSpell : Spell
{
    [SerializeField] private MagicMissileBehavior magicMissile;
    [SerializeField] private SpellHandler missileSpawnLocation;
    [SerializeField] private bool isOverhand;


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

    private MagicWeapon wand;

    private void Awake()
    {
        magicMissile = Resources.Load<MagicMissileBehavior>("Player/Weapons/Spells/MagicMissile/MagicMissilePrefab");
        missileSpawnLocation = GetComponentInParent<SpellHandler>();
        wand = GetComponentInParent<MagicWeapon>();
    }

    public override void CastSpell()
    {
        _castMagicMissile(magicMissile);
    }

    private void _castMagicMissile(MagicMissileBehavior missilePrefab)
    {
        var currentSpawnLocation = missileSpawnLocation.transform.position;
        var castMagicMissile = Instantiate(missilePrefab, currentSpawnLocation, Quaternion.identity);
        castMagicMissile.SetSpellSettings(missileTravelSpeed, heightDividend, offsetX, offsetY, homingRotationSpeed, homingMissileSpeed, angleOfObject, missileSpeedDecreaseOvertime, missileRotateSpeedIncreaseOvertime, lightIntensityOnDeath);
        castMagicMissile.Init(wand.MouseAttackPosition, currentSpawnLocation, _determineTrajectorySide());
    }

    private bool _determineTrajectorySide()
    {
        switch (wand.Swing)
        {
            case 1:
                isOverhand = !wand.isLookingLeft;
                break;
            case -1:
                isOverhand = wand.isLookingLeft;
                break;
        }
        return isOverhand;
    }
}
