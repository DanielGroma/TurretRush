using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine.InputSystem;

public class TurretShooter : MonoBehaviour
{ 
    [SerializeField] private Transform _firePoint;

    private TurretConfig _turretConfig;
    private IPool<Projectile> _projectilePool;
    private TurretController _controller;
    private InputHandler _input;

    private bool canShoot = true;

    [Inject]
    public void Construct(IPool<Projectile> projectilePool, TurretController controller, InputHandler input, TurretConfig turretConfig)
    {
        _turretConfig = turretConfig;
        _projectilePool = projectilePool;
        _controller = controller;
        _input = input;
    }

    private void Update()
    {
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
        ResetCooldownAsync().Forget();
    }

    private async UniTaskVoid ResetCooldownAsync()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(_turretConfig.fireRate));
        canShoot = true;
    }
}