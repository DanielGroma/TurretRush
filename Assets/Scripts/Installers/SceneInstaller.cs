using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameStateManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameStateHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameUIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelRestarter>().FromComponentInHierarchy().AsSingle();
    }
}
