using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameStateManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CarMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CarStateHandler>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ProjectilePool>().FromComponentInHierarchy().AsSingle();

        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle();

        Container.Bind<TurretController>().FromComponentInHierarchy().AsSingle();
    }
}