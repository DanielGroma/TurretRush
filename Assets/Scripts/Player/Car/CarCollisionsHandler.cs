using UnityEngine;
using Zenject;

public class CarCollisionsHandler : MonoBehaviour
{
    private IDamageable _carHealth;

    [Inject]
    public void Construct(IDamageable carHealth)
    {
        _carHealth = carHealth;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null)
                return;
            if (enemy.IsDead)
                return;
            _carHealth.TakeDamage(enemy.EnemyDamage);
            enemy?.TakeDamage(enemy.CollisionDamage);
        }
    }
}
