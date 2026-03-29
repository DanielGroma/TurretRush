using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private Transform _carTransform;
    [SerializeField] private LayerMask _roadLayer;

    public override void InstallBindings()
    {
        Container.Bind<EnemyConfig>().FromInstance(_enemyConfig).AsSingle();

        Container.Bind<ISpawnPositionProvider>()
            .To<SpawnPositionProvider>()
            .AsSingle()
            .WithArguments(_carTransform, 70f, 8f, _roadLayer);

        Container.Bind<IPool<Enemy>>()
            .To<EnemyPool>()
            .AsSingle()
            .WithArguments(
                new System.Func<Enemy>(() =>
                    Container.InstantiatePrefabForComponent<Enemy>(_enemyPrefab)),
                20
            );

        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
    }
}