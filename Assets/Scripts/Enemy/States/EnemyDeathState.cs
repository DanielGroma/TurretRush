using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyDeathState : IEnemyState
{
    private Enemy _enemy;

    public EnemyDeathState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter() => HandleDeathAsync().Forget();

    private async UniTaskVoid HandleDeathAsync()
    {
        _enemy.animationHandler.PlayAnimation("Death");
        await UniTask.Delay(1500);
        _enemy.Despawn();
    }

    public void Update() { }
    public void Exit() 
    {
        _enemy.animationHandler.StopAnimation();
    }
}