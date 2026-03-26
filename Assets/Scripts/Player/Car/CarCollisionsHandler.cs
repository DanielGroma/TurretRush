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
            enemy?.Die();
            _damageTaker.TakeDamage(enemy.EnemyDamage);
        }
    }
}
