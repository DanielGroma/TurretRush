using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Transform _headTransform;
    private Camera _camera;

    private float _targetValue = 1f;
    private float _currentValue = 1f;

    [SerializeField] private float _smoothSpeed = 5f;

    public void Init(Transform target)
    {
        _headTransform = target;
        _camera = Camera.main;
    }

    public void SetHealth(float current, float max)
    {
        _targetValue = current / max;
    }

    private void LateUpdate()
    {
        if (_headTransform == null || _camera == null) return;

        transform.position = _headTransform.position - new Vector3(0, 2.5f, 0);
        transform.forward = _camera.transform.forward;

        _currentValue = Mathf.Lerp(_currentValue, _targetValue, Time.deltaTime * _smoothSpeed);

        if (Mathf.Abs(_currentValue - _targetValue) < 0.01f)
            _currentValue = _targetValue;

        _slider.value = _currentValue;
    }

    private void OnEnable()
    {
        _currentValue = 1f;
        _targetValue = 1f;

        if (_slider != null)
            _slider.value = 1f;
    }
}