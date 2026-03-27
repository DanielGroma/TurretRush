using UnityEngine;

public class CarVFXController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private Transform _carVisualRoot;

    [Header("Emission")]
    [SerializeField] private float _minEmission = 0f;
    [SerializeField] private float _maxEmission = 40f;

    [Header("Particle Shape")]
    [SerializeField] private float _minSize = 0.2f;
    [SerializeField] private float _maxSize = 1.2f;

    [SerializeField] private float _minLifetime = 0.4f;
    [SerializeField] private float _maxLifetime = 1.5f;

    [SerializeField] private float _minSpeed = 0.2f;
    [SerializeField] private float _maxSpeed = 0.6f;

    [Header("Smoke Color")]
    [SerializeField] private Color _healthyColor = new Color(0.4f, 0.4f, 0.4f, 0.2f);
    [SerializeField] private Color _damagedColor = new Color(0.12f, 0.1f, 0.08f, 0.9f);

    [Header("Damage Burst")]
    [SerializeField] private int _damageBurstMin = 2;
    [SerializeField] private int _damageBurstMax = 8;

    [Header("Critical State")]
    [SerializeField] private float _criticalHealthThreshold = 0.2f;
    [SerializeField] private int _criticalBurstCount = 10;
    [SerializeField] private float _criticalBurstCooldown = 0.35f;

    [Header("Shake")]
    [SerializeField] private float _maxShakeOffset = 0.05f;
    [SerializeField] private float _maxShakeRotation = 1.5f;
    [SerializeField] private float _shakeSpeed = 18f;

    private float _currentNormalizedHealth = 1f;
    private float _lastNormalizedHealth = 1f;
    private float _criticalTimer = 0f;

    private Vector3 _initialLocalPosition;
    private Quaternion _initialLocalRotation;

    private void Awake()
    {
        if (_carVisualRoot != null)
        {
            _initialLocalPosition = _carVisualRoot.localPosition;
            _initialLocalRotation = _carVisualRoot.localRotation;
        }

        SetHealthNormalized(1f);
    }

    private void Update()
    {
        HandleCriticalSmoke();
        HandleShake();
    }

    public void SetHealthNormalized(float normalizedHealth)
    {
        if (_smoke == null)
            return;

        normalizedHealth = Mathf.Clamp01(normalizedHealth);
        float damage01 = 1f - normalizedHealth;

        var emission = _smoke.emission;
        var main = _smoke.main;

        emission.rateOverTime = Mathf.Lerp(_minEmission, _maxEmission, damage01);
        main.startSize = Mathf.Lerp(_minSize, _maxSize, damage01);
        main.startLifetime = Mathf.Lerp(_minLifetime, _maxLifetime, damage01);
        main.startSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, damage01);

        Color smokeColor = Color.Lerp(_healthyColor, _damagedColor, damage01);
        main.startColor = smokeColor;

        if (damage01 > 0.05f)
            _smoke.Play();
        else
            _smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _currentNormalizedHealth = normalizedHealth;
    }

    public void PlayDamageBurst(float previousHealth, float currentHealth)
    {
        if (_smoke == null)
            return;

        float damageDelta = Mathf.Clamp01(previousHealth - currentHealth);
        if (damageDelta <= 0f)
            return;

        int burstCount = Mathf.RoundToInt(Mathf.Lerp(_damageBurstMin, _damageBurstMax, damageDelta));
        _smoke.Emit(burstCount);
    }

    private void HandleCriticalSmoke()
    {
        if (_smoke == null)
            return;

        if (_currentNormalizedHealth > _criticalHealthThreshold)
        {
            _criticalTimer = 0f;
            return;
        }

        _criticalTimer -= Time.deltaTime;
        if (_criticalTimer > 0f)
            return;

        _criticalTimer = _criticalBurstCooldown;
        _smoke.Emit(_criticalBurstCount);
    }

    private void HandleShake()
    {
        if (_carVisualRoot == null)
            return;

        float damage01 = 1f - _currentNormalizedHealth;

        if (damage01 < 0.4f)
        {
            _carVisualRoot.localPosition = Vector3.Lerp(
                _carVisualRoot.localPosition,
                _initialLocalPosition,
                Time.deltaTime * 10f);

            _carVisualRoot.localRotation = Quaternion.Slerp(
                _carVisualRoot.localRotation,
                _initialLocalRotation,
                Time.deltaTime * 10f);

            return;
        }

        float shake01 = Mathf.InverseLerp(0.4f, 1f, damage01);

        Vector3 offset = new Vector3(
            Mathf.Sin(Time.time * _shakeSpeed) * _maxShakeOffset * shake01,
            Mathf.Cos(Time.time * (_shakeSpeed * 0.85f)) * _maxShakeOffset * 0.35f * shake01,
            0f);

        Vector3 rotOffset = new Vector3(
            0f,
            0f,
            Mathf.Sin(Time.time * (_shakeSpeed * 0.9f)) * _maxShakeRotation * shake01);

        _carVisualRoot.localPosition = _initialLocalPosition + offset;
        _carVisualRoot.localRotation = _initialLocalRotation * Quaternion.Euler(rotOffset);
    }

    public void ResetState()
    {
        _currentNormalizedHealth = 1f;
        _lastNormalizedHealth = 1f;
        _criticalTimer = 0f;

        SetHealthNormalized(1f);

        if (_carVisualRoot != null)
        {
            _carVisualRoot.localPosition = _initialLocalPosition;
            _carVisualRoot.localRotation = _initialLocalRotation;
        }
    }
}