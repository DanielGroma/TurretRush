using UnityEngine;
using Zenject;

public class GameStateHandler : MonoBehaviour
{
    private CarMovement _carMovement;
    private GameStateManager _gameStateManager;
    private InputHandler _input;
    private GameUIController _gameUIController;
    private EnemySpawner _enemySpawner;

    [Inject]
    public void Construct(
        CarMovement carMovement,
        GameStateManager gameStateManager,
        InputHandler input,
        GameUIController gameUIController,
        EnemySpawner enemySpawner)
    {
        _carMovement = carMovement;
        _gameStateManager = gameStateManager;
        _input = input;
        _gameUIController = gameUIController;
        _enemySpawner = enemySpawner;
    }

    private void Update()
    {
        if (_gameStateManager.CurrentState != GameState.Idle)
            return;

        if (_input.IsPressedThisFrame)
        {
            _gameUIController.Hide();
            _gameStateManager.SetState(GameState.Playing);
        }
    }

    private void OnEnable()
    {
        _gameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        _gameStateManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                _carMovement.StartMoving();
                break;

            case GameState.Lose:
                _carMovement.StopMoving();
                _enemySpawner.StopAllActiveEnemies();
                _gameUIController.ShowLose();
                break;

            case GameState.Win:
                _carMovement.StopMoving();
                _enemySpawner.KillAllActiveEnemies();
                _gameUIController.ShowWin();
                break;
        }
    }
}