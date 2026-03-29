using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class EnemyDeathState : IEnemyState
{
    private readonly Enemy _enemy;

    public EnemyDeathState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        HandleDeathAsync(_enemy.destroyCancellationToken).Forget();
    }

    private async UniTask HandleDeathAsync(CancellationToken token)
    {
        _enemy.AnimationHandler.PlayAnimation("Death");

        try
        {
            await UniTask.Delay(1500, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (_enemy == null)
            return;

        _enemy.Despawn();
    }

    public void Update() { }
}