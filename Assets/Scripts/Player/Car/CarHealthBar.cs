using UnityEngine;
using UnityEngine.UI;

public class CarHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private float _target = 1f;
    private float _current = 1f;

    [SerializeField] private float _smoothSpeed = 5f;

    public void SetHealth(float current, float max)
    {
        _target = current / max;
    }

    private void Update()
    {
        _current = Mathf.MoveTowards(_current, _target, _smoothSpeed * Time.deltaTime);
        _slider.value = _current;
    }

    public void ResetBar()
    {
        _current = 1f;
        _target = 1f;
        _slider.value = 1f;
    }
}