using UnityEngine;
using Zenject;

public class TurretAimVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private Color _rayColor = Color.red;
    [SerializeField] private GameObject _beamPrefab;

    private TurretController _controller;
    private GameObject _beamInstance;

    [Inject]
    public void Construct(TurretController controller)
    {
        _controller = controller;
    }

    private void Start()
    {
        if (_beamPrefab != null)
        {
            _beamInstance = Instantiate(_beamPrefab);
            _beamInstance.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
        }
    }

    private void Update()
    {
        if (_controller == null || !_controller.IsActive || _firePoint == null || _beamInstance == null)
        {
            if (_beamInstance != null)
                _beamInstance.SetActive(false);
            return;
        }

        _beamInstance.SetActive(true);

        Vector3 direction = _firePoint.forward;
        _beamInstance.transform.position = _firePoint.position + direction * (_maxDistance / 2f);
        _beamInstance.transform.rotation = Quaternion.LookRotation(direction);
        Vector3 localScale = _beamInstance.transform.localScale;
        localScale.z = _maxDistance;
        _beamInstance.transform.localScale = localScale;

        if (_beamInstance.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = _rayColor;
        }

        Debug.DrawRay(_firePoint.position, direction * _maxDistance, _rayColor);
    }
}