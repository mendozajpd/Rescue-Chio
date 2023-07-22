using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileSpell : Spell
{
    [SerializeField] private MagicMissileBehavior magicMissile;
    [SerializeField] private SpellHandler missileSpawnLocation;
    [SerializeField] private bool isOverhand;

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
