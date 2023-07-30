using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Spell : MonoBehaviour
{
    public string SpellName;
    public SpellHandler spellHandler;
    public SpellChargeHandler spellCharge;
    public MagicWeapon wand;

    public bool CanSwing;
    public float WeaponAngle;
    public bool CanRotate;

    private PlayerInputActions playerControls;
    public InputAction Charging;

    // Charge

    public virtual void CastSpell()
    {
        Debug.Log("Spell casted");
    }

    public void SetSpellVariables()
    {
        spellHandler = GetComponentInParent<SpellHandler>();
        wand = spellHandler.Wand;
    }

    public void SetMagicWeaponActions(bool swingable, float weaponAngle, bool rotate)
    {
        CanSwing = swingable;
        WeaponAngle = weaponAngle;
        CanRotate = rotate;
    }

    public void SetChargingInputActions(Spell spell)
    {
        playerControls = new PlayerInputActions();
        Charging = playerControls.Player.HoldFire;
        Charging.Enable();
        wand.SpellCharge.Spells.Add(spell);
    }

    public void DisableChargingInputActions(Spell spell)
    {
        Charging.Disable();
        wand.SpellCharge.Spells.Remove(spell);
    }

    public void SetSpellChargeHandler(SpellChargeHandler chargeHandler)
    {
        spellCharge = chargeHandler;
    }

}
