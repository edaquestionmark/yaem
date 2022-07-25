using System;

public class Health : IHealth
{
    private int _maxHealth;
    private int _currentHealth;

    public event Action<DamageArgs> OnDamage;
    public event Action<DamageArgs> OnDeath;

    public Health(int maxHealth, int currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    public Health(int maxHealth) : this(maxHealth, maxHealth) { }

    public int Max => _maxHealth;

    public int Current => _currentHealth;

    public virtual void TakeDamage(DamageArgs args)
    {
        _currentHealth -= args.Damage;

        if(_currentHealth < 0)
        {
            OnDeath?.Invoke(args);
            return;
        }

        OnDamage?.Invoke(args);
    }
}
