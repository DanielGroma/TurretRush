using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private EnemyStateMachine _stateMachine;

    private IEnemyState _idleState;
    private IEnemyState _moveState;
    private IEnemyState _deathState;

    private EnemyConfig _config;
    private System.Action<Enemy> _onDespawn;
    private float _health;

    public float EnemyDamage => _config.damage;


    [SerializeField] private AnimationsHandler _animationHandler;

    public AnimationsHandler animationHandler
    {
        get { return _animationHandler; }
    }


    public void Init(EnemyConfig config, System.Action<Enemy> onDespawn, Transform target)
    {
        _config = config;
        _onDespawn = onDespawn;

        _health = _config.maxHealth;

        SetupStates(target);
        _stateMachine.SetState(_idleState);
        transform.gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void SetupStates(Transform target)
    {
        _stateMachine = new EnemyStateMachine();

        _idleState = new EnemyIdleState(this, target, _config);
        _moveState = new EnemyMoveState(this, _config, target);
        _deathState = new EnemyDeathState(this);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void StartChasing() => _stateMachine.SetState(_moveState);
    public void Die() => _stateMachine.SetState(_deathState);
    public void Despawn() => _onDespawn?.Invoke(this);

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();

    }
}