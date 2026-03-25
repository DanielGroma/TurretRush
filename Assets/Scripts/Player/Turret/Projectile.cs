using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _lifeTime = 5f;

    private ProjectilePool _pool;
    private Vector3 _direction;

    public void SetPool(ProjectilePool pool)
    {
        _pool = pool;
    }

    // Тепер Launch приймає напрямок
    public void Launch(Vector3 direction)
    {
        _direction = direction.normalized;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.TakeDamage(_damage);
            ReturnToPool();
        }*/
    }

    private void ReturnToPool()
    {
        CancelInvoke();
        _pool?.Return(this);
    }
}