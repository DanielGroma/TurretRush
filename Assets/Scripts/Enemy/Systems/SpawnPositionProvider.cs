using UnityEngine;

public class SpawnPositionProvider : ISpawnPositionProvider
{
    private Transform _carTransform;
    private Camera _camera;
    private float _spawnDistance;
    private float _horizontalRange;
    private LayerMask _roadLayer;

    public SpawnPositionProvider(Transform carTransform, Camera camera, float spawnDistance, float horizontalRange, LayerMask roadLayer)
    {
        _carTransform = carTransform;
        _camera = camera;
        _spawnDistance = spawnDistance;
        _horizontalRange = horizontalRange;
        _roadLayer = roadLayer;
    }

    public Vector3 GetPosition()
    {
        return GetRandomRoadPosition(_carTransform.position + _carTransform.forward * _spawnDistance);
    }

    public Vector3 GetPositionInView()
    {
        Vector3 carPos = _carTransform.position;
        float offsetZ = Random.Range(16f, 80f);
        float offsetX = Random.Range(-_horizontalRange, _horizontalRange);

        Vector3 spawnPos = carPos + _carTransform.forward * offsetZ + _carTransform.right * offsetX + Vector3.up * 50f;
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f, _roadLayer))
            return hit.point;

        return carPos + Vector3.up * 1f;
    }

    public Vector3 GetPositionOutOfView()
    {
        Vector3 basePos = _carTransform.position + _carTransform.forward * (_spawnDistance + 30f);

        return GetRandomRoadPosition(basePos);
    }

    private Vector3 GetRandomRoadPosition(Vector3 basePos)
    {
        int attempts = 0;
        while (attempts < 5)
        {
            float offsetX = Random.Range(-_horizontalRange, _horizontalRange);
            Vector3 spawnPos = basePos + _carTransform.right * offsetX + Vector3.up * 50f;

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f, _roadLayer))
                return hit.point;

            attempts++;
        }
        return basePos + Vector3.up * 1f;
    }
}