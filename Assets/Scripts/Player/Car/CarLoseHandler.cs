using System;
using Zenject;

public class CarLoseHandler : IInitializable, IDisposable
{
    private readonly CarHealth _carHealth;
    private readonly GameStateManager _gameStateManager;

    public CarLoseHandler(CarHealth carHealth, GameStateManager gameStateManager)
    {
        _carHealth = carHealth;
        _gameStateManager = gameStateManager;
    }

    public void Initialize()
    {
        _carHealth.OnDied += HandleDied;
    }

    public void Dispose()
    {
        _carHealth.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        _gameStateManager.SetState(GameState.Lose);
    }
}