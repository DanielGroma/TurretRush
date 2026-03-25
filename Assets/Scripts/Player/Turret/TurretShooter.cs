using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine.InputSystem;

public class TurretShooter : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.5f;

    private ProjectilePool _projectilePool;
    private TurretController _controller;
    private InputHandler _input;

    private bool _canShoot = true;

    [Inject]
    public void Construct(ProjectilePool projectilePool, TurretController controller, InputHandler input)
    {
        _projectilePool = projectilePool;
        _controller = controller;
        _input = input;
    }

    private void Update()
    {
        if (!_controller.IsActive || !_canShoot)
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
        proj.SetPool(_projectilePool);
        proj.Launch(shootDirection);

        _canShoot = false;
        ResetCooldownAsync().Forget();
    }

    private async UniTaskVoid ResetCooldownAsync()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(_fireRate));
        _canShoot = true;
    }
}