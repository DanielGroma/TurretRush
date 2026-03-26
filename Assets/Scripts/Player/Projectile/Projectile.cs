using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    private ProjectileConfig _projectileConfig;
    private ProjectilePool _pool;
    private Vector3 _direction;

    [Inject]
    public void Construct(ProjectileConfig projectileConfig)
    {
        _projectileConfig = projectileConfig;
    }

    public void SetPool(ProjectilePool pool)
    {
        _pool = pool;
    }

    public void Launch(Vector3 direction)
    {
        _direction = direction.normalized;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _projectileConfig.lifeTime);
    }

    private void Update()
    {
        transform.position += _direction * _projectileConfig.speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(_projectileConfig.damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        CancelInvoke();
        _pool?.Return(this);
    }
}