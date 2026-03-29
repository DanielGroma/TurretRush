using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CarHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private CarHealth _carHealth;

    [Inject]
    public void Construct(CarHealth carHealth)
    {
        _carHealth = carHealth;
    }

    private void OnEnable()
    {
        if (_carHealth != null)
            _carHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (_carHealth != null)
            _carHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        SetHealth(_carHealth.CurrentHealth, _carHealth.MaxHealth);
    }

    private void HandleHealthChanged(float previousHealth, float currentHealth)
    {
        SetHealth(currentHealth, _carHealth.MaxHealth);
    }

    public void SetHealth(float current, float max)
    {
        if (_slider == null || max <= 0f)
            return;

        _slider.value = current / max;
    }
}