using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    private IPool<Enemy> _pool;
    private ISpawnPositionProvider _positionProvider;
    private EnemyConfig _config;
    private GameStateManager _gameStateManager;

    private readonly List<Enemy> _activeEnemies = new();

    [SerializeField] private Transform _target;
    [SerializeField] private int _initialCount = 1;
    [SerializeField] private int _spawnDelayMs = 2000;

    [Inject]
    public void Construct(
        IPool<Enemy> pool,
        ISpawnPositionProvider provider,
        EnemyConfig config,
        GameStateManager gameStateManager)
    {
        _pool = pool;
        _positionProvider = provider;
        _config = config;
        _gameStateManager = gameStateManager;
    }

    public IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies;

    private void Start()
    {
        for (int i = 0; i < _initialCount; i++)
            SpawnInView();

        SpawnLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask SpawnLoop(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(_spawnDelayMs, cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                if (_gameStateManager.CurrentState == GameState.Playing)
                    SpawnOutOfView();

                await UniTask.Delay(_spawnDelayMs, cancellationToken: token);
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private void SpawnInView()
    {
        var enemy = _pool.Get();
        enemy.transform.position = _positionProvider.GetPositionInView();
        enemy.Init(_config, OnEnemyDespawn, _target, _gameStateManager);
        enemy.gameObject.SetActive(true);

        _activeEnemies.Add(enemy);
    }

    private void SpawnOutOfView()
    {
        var enemy = _pool.Get();
        enemy.transform.position = _positionProvider.GetPositionOutOfView();
        enemy.Init(_config, OnEnemyDespawn, _target, _gameStateManager);
        enemy.gameObject.SetActive(true);

        _activeEnemies.Add(enemy);
    }

    private void OnEnemyDespawn(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
        _pool?.Return(enemy);
    }

    public void KillAllActiveEnemies()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            if (_activeEnemies[i] != null && _activeEnemies[i].gameObject.activeSelf)
                _activeEnemies[i].Die();
        }
    }
    public void StopAllActiveEnemies()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            if (_activeEnemies[i] != null && _activeEnemies[i].gameObject.activeSelf)
                _activeEnemies[i].StopChasing();
        }
    }
}