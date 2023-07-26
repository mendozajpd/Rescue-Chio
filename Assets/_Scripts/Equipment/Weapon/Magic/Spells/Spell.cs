using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public string SpellName;
    public SpellHandler spellHandler;
    public MagicWeapon wand;

    public bool CanSwing;
    public float WeaponAngle;

    public virtual void CastSpell()
    {
        Debug.Log("Spell casted");
    }

    public void SetSpellVariables()
    {
        spellHandler = GetComponentInParent<SpellHandler>();
        wand = spellHandler.Wand;
    }

    public void SetMagicWeaponActions(bool swingable, float weaponAngle)
    {
        CanSwing = swingable;
        WeaponAngle = weaponAngle;
    }

}
