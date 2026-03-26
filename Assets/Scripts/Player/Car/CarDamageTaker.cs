
using Zenject;

class CarDamageTaker : IDamageable
{
    private PlayerConfig _config;
    private GameStateManager _gameStateManager;
    private float _health;

   public CarDamageTaker(PlayerConfig config, GameStateManager gameStateManager)
    {
        _config = config;
        _gameStateManager = gameStateManager;
        _health = _config.maxHealth;

    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            _gameStateManager.SetState(GameState.Lose);
    }
}
