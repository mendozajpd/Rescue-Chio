using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Spell : MonoBehaviour
{
    public string SpellID;
    public string SpellName;
    protected float spellDamage;
    protected float spellKnockback;
    protected float spellCastSpeed;
    protected float spellManaCost;
    public SpellHandler spellHandler;
    public SpellChargeHandler spellCharge;
    public MagicWeapon wand;

    [Header("Wand Settings")]
    public bool CanSwing;
    public float WeaponAngle;
    public bool CanRotate;

    private PlayerInputActions playerControls;
    public InputAction Charging;

    public float SpellDamage 
    { 
        get => spellDamage; 
        set => spellDamage = value; 
    }
    public float SpellKnockback 
    { 
        get => spellKnockback; 
        set => spellKnockback = value; 
    }
    public float SpellCastSpeed 
    { 
        get => spellCastSpeed; 
        set => spellCastSpeed = value; 
    }

    public float SpellManaCost
    {
        get => spellManaCost;
        set => spellManaCost = value;
    }

    public virtual void CastSpell()
    {
        //Debug.Log("casted!");
    }

    public void SetSpellVariables(float defaultSpellDamage, float defaultSpellKB, float defaultCastSpeed, float defaultManaCost)
    {
        SpellDamage = defaultSpellDamage;
        SpellKnockback = defaultSpellKB;
        SpellCastSpeed = defaultCastSpeed;
        SpellManaCost = defaultManaCost;
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
        SetSpellChargeHandler(wand.SpellCharge);
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
