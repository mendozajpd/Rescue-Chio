using UnityEngine;

public interface IDamageable 
{
    public void Damage(float damageAmount, bool isCrit, float iTime, Attack attack, Color32 color);
}
