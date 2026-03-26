using System.Collections.Generic;

public class ProjectilePool : IPool<Projectile>
{
    private readonly Queue<Projectile> _pool = new();
    private readonly System.Func<Projectile> _createFunc;

    public ProjectilePool(System.Func<Projectile> createFunc, int size)
    {
        _createFunc = createFunc;

        for (int i = 0; i < size; i++)
        {
            var p = Create();
            p.gameObject.SetActive(false);
            _pool.Enqueue(p);
        }
    }

    private Projectile Create()
    {
        var p = _createFunc();
        p.SetPool(this);
        p.gameObject.SetActive(false);
        return p;
    }
    public Projectile Get()
    {
        var proj = _pool.Count > 0 ? _pool.Dequeue() : Create();
        proj.gameObject.SetActive(true);
        return proj;
    }
    public void Return(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        _pool.Enqueue(proj);
    }
}