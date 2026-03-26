using UnityEngine;
using Zenject;

public class CarStateHandler : MonoBehaviour
{
    private CarMovement _carMovement;
    private GameStateManager _gameStateManager;
    private InputHandler _input;

    [Inject]
    public void Construct(
        CarMovement carMovement,
        GameStateManager gameStateManager,
        InputHandler input)
    {
        _carMovement = carMovement;
        _gameStateManager = gameStateManager;
        _input = input;
    }

    private void Update()
    {
        if (_gameStateManager.CurrentState != GameState.Idle)
            return;

        if (_input.IsPressedThisFrame)
        {
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
        if (newState == GameState.Playing)
            _carMovement.StartMoving();
        else if(newState == GameState.Lose)
        {
            _carMovement.StopMoving();
            Time.timeScale = 0;
            Destroy(transform.gameObject);
        }
    }
}