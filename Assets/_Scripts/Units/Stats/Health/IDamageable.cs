public interface IDamageable 
{
    public void DamageCrittable(float damageAmount, bool isCrit, Attack attack);
    public void Damage(float damageAmount, Attack attack);
}
