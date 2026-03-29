using System;
using UnityEngine;

public class CarHealth : IDamageable
{
    private readonly PlayerConfig _config;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => _config.maxHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    public CarHealth(PlayerConfig config)
    {
        _config = config;
        CurrentHealth = _config.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f)
            return;

        float previousHealth = CurrentHealth;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f);

        OnHealthChanged?.Invoke(previousHealth, CurrentHealth);

        if (CurrentHealth <= 0f)
            OnDied?.Invoke();
    }

    public void Reset()
    {
        CurrentHealth = _config.maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, CurrentHealth);
    }
}