using UnityEngine;
using Zenject;

class CarCollisionsHandler : MonoBehaviour
{
    private CarDamageTaker _damageTaker;

    [Inject]
    public void Construct(CarDamageTaker damageTaker)
    {
        _damageTaker = damageTaker;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.IsDead)
                return;
            enemy?.TakeDamage(20);
            _damageTaker.TakeDamage(enemy.EnemyDamage);
        }
    }
}
