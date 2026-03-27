using Zenject;
using UnityEngine;

class CarDamageTaker : IDamageable
{
    private PlayerConfig _config;
    private GameStateManager _gameStateManager;
    private CarHealthBar _healthBar;
    private CarVFXController _smokeController;

    private float _health;

    public CarDamageTaker(
        PlayerConfig config,
        GameStateManager gameStateManager,
        CarHealthBar healthBar,
        CarVFXController smokeController)
    {
        _config = config;
        _gameStateManager = gameStateManager;
        _healthBar = healthBar;
        _smokeController = smokeController;

        _health = _config.maxHealth;
        _healthBar.SetHealth(_health, _config.maxHealth);
        _smokeController.SetHealthNormalized(_health / _config.maxHealth);
    }

    public void TakeDamage(float damage)
    {
        float previousHealth = _health;

        _health -= damage;
        _health = Mathf.Max(_health, 0f);

        UpdateView(previousHealth, _health);

        if (_health <= 0)
            _gameStateManager.SetState(GameState.Lose);
    }

    private void UpdateView(float previousHealth, float currentHealth)
    {
        _healthBar.SetHealth(currentHealth, _config.maxHealth);

        float previousNormalized = previousHealth / _config.maxHealth;
        float currentNormalized = currentHealth / _config.maxHealth;

        _smokeController.SetHealthNormalized(currentNormalized);
        _smokeController.PlayDamageBurst(previousNormalized, currentNormalized);
    }
}