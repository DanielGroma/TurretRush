using System.Collections.Generic;

public class EnemyPool : IPool<Enemy>
{
    private Queue<Enemy> _pool = new();
    private readonly System.Func<Enemy> _createFunc;

    public EnemyPool(System.Func<Enemy> createFunc, int initialSize)
    {
        _createFunc = createFunc;

        for (int i = 0; i < initialSize; i++)
        {
            var e = Create();
            e.gameObject.SetActive(false);
            _pool.Enqueue(e);
        }
    }

    private Enemy Create()
    {
        var e = _createFunc();
        e.gameObject.SetActive(false);
        return e;
    }

    public Enemy Get() => _pool.Count > 0 ? _pool.Dequeue() : Create();
    public void Return(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _pool.Enqueue(enemy);
    }
}