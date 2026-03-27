using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Cysharp.Threading.Tasks;

public class LevelRestarter : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    private InputHandler _input;

    private bool _canRestart = false;

    [Inject]
    public void Construct(GameStateManager gameStateManager, InputHandler input)
    {
        _gameStateManager = gameStateManager;
        _input = input;
    }

    private async void OnEnable()
    {
        await UniTask.Delay(300);
        _canRestart = true;
    }

    private void Update()
    {
        if (!_canRestart)
            return;

        if (_gameStateManager.CurrentState != GameState.Win &&
            _gameStateManager.CurrentState != GameState.Lose)
            return;

        if (_input.IsPressedThisFrame)
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}