using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStats", menuName = "ScriptableObjects/Units/Default Stats")]
public class DefaultStatsSO : ScriptableObject
{
    [Header("General Variables")]
    public string UnitName;

    [Header("Unit Default Stats")]
    public float DefaultMaxHealth;
    public float DefaultMaxMana;
    public float DefaultAggro;
    public float DefaultAttackSpeed;
    public float DefaultCritHitChance;
    public float DefaultBaseDamage; // idk about this, change in the future maybe
    public float DefaultDefense;
    public float DefaultKnockback;
    public float DefaultKnockbackResistance;
    public float DefaultMoveSpeed;
}
