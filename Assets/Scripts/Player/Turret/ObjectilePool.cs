using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _poolSize = 10;

    private Queue<Projectile> _pool = new Queue<Projectile>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var proj = Instantiate(_projectilePrefab, transform);
            proj.gameObject.SetActive(false);
            _pool.Enqueue(proj);
        }
    }

    public Projectile Get()
    {
        if (_pool.Count > 0)
        {
            var proj = _pool.Dequeue();
            proj.gameObject.SetActive(true);
            return proj;
        }
        else
        {
            var proj = Instantiate(_projectilePrefab, transform);
            return proj;
        }
    }

    public void Return(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        _pool.Enqueue(proj);
    }
}