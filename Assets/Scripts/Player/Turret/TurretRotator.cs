using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class TurretRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _maxAngle = 45f;

    private GameStateManager _gameStateManager;
    private InputHandler _input;
    private float _currentAngle = 0f;

    private bool canUseTurret = false;

    [Inject]
    public void Construct(GameStateManager gameStateManager, InputHandler input)
    {
        _gameStateManager = gameStateManager;
        _input = input;
    }

    private void Update()
    {
        if (_gameStateManager.CurrentState != GameState.Playing)
            return;

        if (!canUseTurret)
        {
            if (_input.IsPressedThisFrame)
                canUseTurret = true;

            return;
        }

        if (!_input.IsHolding)
            return;

        RotateTurret();
    }
    private void RotateTurret()
    {
        float deltaX = 0f;

        if (Mouse.current != null)
            deltaX = Mouse.current.delta.ReadValue().x;
        else if (Touchscreen.current != null)
            deltaX = Touchscreen.current.primaryTouch.delta.ReadValue().x;

        _currentAngle += deltaX * _rotationSpeed * Time.deltaTime;

        _currentAngle = Mathf.Clamp(_currentAngle, - _maxAngle, _maxAngle);

        Vector3 forward = transform.parent.forward;
        Quaternion targetRotation = Quaternion.Euler(0, _currentAngle, 0) * Quaternion.LookRotation(forward);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime);
    }
}