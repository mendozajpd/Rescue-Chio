using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public string SpellName;
    public SpellHandler spellHandler;
    public MagicWeapon wand;
    public virtual void CastSpell()
    {
        Debug.Log("Spell casted");
    }

    public void SetSpellVariables()
    {
        spellHandler = GetComponentInParent<SpellHandler>();
        wand = spellHandler.Wand;
    }

}
