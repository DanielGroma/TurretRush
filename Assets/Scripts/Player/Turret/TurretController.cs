using UnityEngine;
using Zenject;

public class TurretController : MonoBehaviour
{
    private InputHandler _input;
    private GameStateManager _gameStateManager;

    public bool IsActive { get; private set; } = false;

    [Inject]
    public void Construct(InputHandler input, GameStateManager gameStateManager)
    {
        _input = input;
        _gameStateManager = gameStateManager;
    }

    private void Update()
    {
        if (_gameStateManager.CurrentState != GameState.Playing)
            return;

        if (!IsActive && _input.IsPressedThisFrame)
        {
            IsActive = true;
        }
    }
}