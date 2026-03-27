using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyVFXController _enemyVFXController;
    [SerializeField] private AnimationsHandler _animationHandler;
    [SerializeField] private EnemyHealthBar _healthBar;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private float _despawnDistanceBehind = 20f;

    private IEnemyState _idleState;
    private IEnemyState _moveState;
    private IEnemyState _deathState;

    private EnemyStateMachine _stateMachine;
    private EnemyConfig _config;
    private System.Action<Enemy> _onDespawn;
    private GameStateManager _gameStateManager;
    private Transform _carTransform;

    private bool isDead;
    public bool IsDead => isDead;
    public float EnemyDamage => _config.damage;
    private float _health;


    public AnimationsHandler animationHandler
    {
        get { return _animationHandler; }
    }


    public void Init(
        EnemyConfig config,
        System.Action<Enemy> onDespawn,
        Transform target,
        GameStateManager gameStateManager)
    {
        _config = config;
        _onDespawn = onDespawn;
        _gameStateManager = gameStateManager;

        _health = _config.maxHealth;
        _carTransform = target;
        SetupStates(target);
        _stateMachine.SetState(_idleState);
        transform.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        isDead = false;
        _healthBar.Init(_headTransform.transform);
        _healthBar.SetHealth(_config.maxHealth, _config.maxHealth);
    }

    private void SetupStates(Transform target)
    {
        _stateMachine = new EnemyStateMachine();

        _idleState = new EnemyIdleState(this, target, _config, _gameStateManager);
        _moveState = new EnemyMoveState(this, _config, target);
        _deathState = new EnemyDeathState(this);
    }

    private void Update()
    {
        _stateMachine.Update();
        CheckBehindDespawn();
    }

    public void StartChasing() => _stateMachine.SetState(_moveState);
    public void StopChasing() => _stateMachine.SetState(_idleState);
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        _stateMachine.SetState(_deathState);
    }
    public void Despawn() => _onDespawn?.Invoke(this);

    private void CheckBehindDespawn()
    {
        if (_carTransform == null || isDead)
            return;

        Vector3 toEnemy = transform.position - _carTransform.position;

        float dot = Vector3.Dot(_carTransform.forward, toEnemy);

        if (dot < 0)
        {
            float distance = toEnemy.magnitude;

            if (distance > _despawnDistanceBehind)
            {
                Despawn();
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        _health -= damage;
        _enemyVFXController.Play();
        _healthBar.SetHealth(_health, _config.maxHealth);
        if (_health <= 0)
            Die();
    }
}