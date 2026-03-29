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

    private EnemyHealth _health;
    private bool isDead;

    public bool IsDead => isDead;
    public float EnemyDamage => _config.damage;
    public float CollisionDamage => _config.collisionDamage;
    public AnimationsHandler AnimationHandler => _animationHandler;

    public void Init(
        EnemyConfig config,
        System.Action<Enemy> onDespawn,
        Transform target,
        GameStateManager gameStateManager)
    {
        _config = config;
        _onDespawn = onDespawn;
        _gameStateManager = gameStateManager;
        _carTransform = target;

        isDead = false;

        SetupStates(target);
        SetupHealth();

        _stateMachine.SetState(_idleState);

        if (_enemyVFXController != null)
            _enemyVFXController.Stop();

        if (_healthBar != null)
        {
            _healthBar.Init(_headTransform);
            _healthBar.SetHealth(_config.maxHealth, _config.maxHealth);
        }
    }

    private void SetupStates(Transform target)
    {
        _stateMachine = new EnemyStateMachine();

        _idleState = new EnemyIdleState(this, target, _config, _gameStateManager);
        _moveState = new EnemyMoveState(this, _config, target);
        _deathState = new EnemyDeathState(this);
    }

    private void SetupHealth()
    {
        if (_health != null)
        {
            _health.OnHealthChanged -= HandleHealthChanged;
            _health.OnDied -= HandleDied;
        }

        _health = new EnemyHealth(_config);
        _health.OnHealthChanged += HandleHealthChanged;
        _health.OnDied += HandleDied;
    }

    private void Update()
    {
        _stateMachine?.Update();
        CheckBehindDespawn();
    }

    public void StartChasing()
    {
        if (isDead)
            return;

        _stateMachine.SetState(_moveState);
    }

    public void StopChasing()
    {
        if (isDead)
            return;

        _stateMachine.SetState(_idleState);
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        _health.TakeDamage(damage);
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        _stateMachine.SetState(_deathState);
    }

    public void Despawn()
    {
        if (_enemyVFXController != null)
            _enemyVFXController.Stop();

        if (_health != null)
        {
            _health.OnHealthChanged -= HandleHealthChanged;
            _health.OnDied -= HandleDied;
        }

        _onDespawn?.Invoke(this);
    }

    private void HandleHealthChanged(float previousHealth, float currentHealth)
    {
        if (_enemyVFXController != null)
            _enemyVFXController.Play();

        if (_healthBar != null)
            _healthBar.SetHealth(currentHealth, _health.MaxHealth);
    }

    private void HandleDied()
    {
        Die();
    }

    private void CheckBehindDespawn()
    {
        if (_carTransform == null || isDead)
            return;

        Vector3 toEnemy = transform.position - _carTransform.position;
        float dot = Vector3.Dot(_carTransform.forward, toEnemy);

        if (dot < 0f && toEnemy.magnitude > _despawnDistanceBehind)
            Despawn();
    }
}