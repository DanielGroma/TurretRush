using UnityEngine;

public enum GameState
{
    Idle,
    Playing,
    Win,
    Lose
}

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; } = GameState.Idle;

    public delegate void GameStateChanged(GameState newState);
    public event GameStateChanged OnGameStateChanged;


    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}