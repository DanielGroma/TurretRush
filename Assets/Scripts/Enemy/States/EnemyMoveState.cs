using UnityEngine;

public class EnemyMoveState : IEnemyState
{
    private Enemy _enemy;
    private EnemyConfig _config;
    private Transform _target;

    public EnemyMoveState(Enemy enemy, EnemyConfig config, Transform target)
    {
        _enemy = enemy;
        _config = config;
        _target = target;
    }

    public void Enter() 
    {
        _enemy.AnimationHandler.PlayAnimation("Walk");
    }

    public void Update()
    {
        if (_target == null) return;

        Vector3 dir = (_target.position - _enemy.transform.position).normalized;
        _enemy.transform.position += dir * _config.moveSpeed * Time.deltaTime;
        Vector3 lookDir = new Vector3(dir.x, 0, dir.z);

        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            _enemy.transform.rotation = Quaternion.Slerp(
                _enemy.transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }
}