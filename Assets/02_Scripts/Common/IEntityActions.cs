using System;

public static class IEntityActions
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
        void Heal(float amount);
    }
}
