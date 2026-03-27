using UnityEngine;
using Zenject;

public class FinishSpawner : MonoBehaviour
{
    [SerializeField] private Transform _carTransform;
    [SerializeField] private GameObject _finishPrefab;
    [SerializeField] private float _finishDistance = 300f;
    [SerializeField] private LayerMask _roadLayer;

    private GameObject _spawnedFinish;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void Start()
    {
        SpawnFinish();
    }

    private void SpawnFinish()
    {
        Vector3 basePos = _carTransform.position + _carTransform.forward * _finishDistance;
        Vector3 spawnPos = GetRoadPosition(basePos);

        _spawnedFinish = _container.InstantiatePrefab(_finishPrefab, spawnPos, Quaternion.identity, null);
        _spawnedFinish.transform.forward = _carTransform.forward;
    }

    private Vector3 GetRoadPosition(Vector3 basePos)
    {
        Vector3 rayStart = basePos + Vector3.up * 50f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, _roadLayer))
            return hit.point;

        return basePos;
    }
}