using System;

public interface IHealth
{
    int Max { get; }
    int Current { get; }
    void TakeDamage(DamageArgs args);

    event Action<DamageArgs> OnDamage;
    event Action<DamageArgs> OnDeath;
}
