using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class LevelRestarter : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    private InputHandler _input;

    private bool canRestart = false;

    [Inject]
    public void Construct(GameStateManager gameStateManager, InputHandler input)
    {
        _gameStateManager = gameStateManager;
        _input = input;
    }

    private void OnEnable()
    {
        canRestart = false;
        EnableRestartAsync(destroyCancellationToken).Forget();
    }

    private async UniTask EnableRestartAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(300, cancellationToken: token);
            canRestart = true;
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private void Update()
    {
        if (!canRestart)
            return;

        if (_gameStateManager.CurrentState != GameState.Win &&
            _gameStateManager.CurrentState != GameState.Lose)
            return;

        if (_input.IsPressedThisFrame)
            RestartLevel();
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}