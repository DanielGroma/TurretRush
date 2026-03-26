using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    private IPool<Enemy> _pool;
    private ISpawnPositionProvider _positionProvider;
    private EnemyConfig _config;

    [SerializeField] private Transform _target;
    [SerializeField] private int _initialCount = 3;
    [SerializeField] private int _spawnDelayMs = 2000;

    [Inject]
    public void Construct(IPool<Enemy> pool, ISpawnPositionProvider provider, EnemyConfig config)
    {
        _pool = pool;
        _positionProvider = provider;
        _config = config;
    }

    private void Start()
    {
        for (int i = 0; i < _initialCount; i++)
            SpawnInView();

        SpawnLoop().Forget();
    }

    private async UniTask SpawnLoop()
    {
        await UniTask.Delay(_spawnDelayMs);
        while (true)
        {
            SpawnOutOfView();
            await UniTask.Delay(_spawnDelayMs);
        }
    }

    private void SpawnInView()
    {
        var enemy = _pool.Get();
        enemy.transform.position = _positionProvider.GetPositionInView();
        enemy.Init(_config, OnEnemyDespawn, _target);
        enemy.gameObject.SetActive(true);
    }

    private void SpawnOutOfView()
    {
        var enemy = _pool.Get();
        enemy.transform.position = _positionProvider.GetPositionOutOfView();
        enemy.Init(_config, OnEnemyDespawn, _target);
        enemy.gameObject.SetActive(true);
    }

    private void OnEnemyDespawn(Enemy enemy)
    {
        _pool.Return(enemy);
    }
}