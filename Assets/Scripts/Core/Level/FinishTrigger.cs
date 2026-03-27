using UnityEngine;
using Zenject;

public class FinishTrigger : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    private EnemySpawner _enemySpawner;

    [Inject]
    public void Construct(GameStateManager gameStateManager, EnemySpawner enemySpawner)
    {
        _gameStateManager = gameStateManager;
        _enemySpawner = enemySpawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _gameStateManager.SetState(GameState.Win);
    }
}