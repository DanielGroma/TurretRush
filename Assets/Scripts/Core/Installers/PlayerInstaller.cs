using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private TurretConfig _turretConfig;
    [SerializeField] private Projectile _projectile;

    public override void InstallBindings()
    {
        Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
        Container.Bind<ProjectileConfig>().FromInstance(_projectileConfig).AsSingle();
        Container.Bind<TurretConfig>().FromInstance(_turretConfig).AsSingle();

        Container.Bind<GameStateManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CarMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CarStateHandler>().FromComponentInHierarchy().AsSingle();

        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IPool<Projectile>>()
            .To<ProjectilePool>()
            .AsSingle()
            .WithArguments(
                new System.Func<Projectile>(() =>
                    Container.InstantiatePrefabForComponent<Projectile>(_projectile)),
                10
            ); 
        
        Container.Bind<TurretController>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CarCollisionsHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CarDamageTaker>().AsSingle();
    }
}