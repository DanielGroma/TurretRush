using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine.InputSystem;
using System.Threading;
using System;

public class TurretShooter : MonoBehaviour
{ 
    [SerializeField] private Transform _firePoint;

    private GameStateManager _stateManager;
    private TurretConfig _turretConfig;
    private IPool<Projectile> _projectilePool;
    private TurretController _controller;
    private InputHandler _input;

    private bool canShoot = true;
    private CancellationToken _destroyToken;


    [Inject]
    public void Construct(
    IPool<Projectile> projectilePool, 
    TurretController controller, 
    InputHandler input, 
    TurretConfig turretConfig, 
    GameStateManager gameStateManager)
    {
        _turretConfig = turretConfig;
        _projectilePool = projectilePool;
        _controller = controller;
        _input = input;
        _stateManager = gameStateManager;
    }
    private void Awake()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();
    }
    private void Update()
    {
        if (_stateManager.CurrentState != GameState.Playing)
            return;

        if (!_controller.IsActive || !canShoot)
            return;

        if (_input.IsHolding)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var proj = _projectilePool.Get();
        proj.transform.position = _firePoint.position;

        Vector3 shootDirection = _firePoint.TransformDirection(Vector3.forward);

        proj.transform.rotation = Quaternion.LookRotation(shootDirection);
        proj.Launch(shootDirection);

        canShoot = false;
        ResetCooldownAsync(_destroyToken).Forget();
    }

    private async UniTask ResetCooldownAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(
                System.TimeSpan.FromSeconds(_turretConfig.fireRate),
                cancellationToken: token);

            canShoot = true;
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }
}