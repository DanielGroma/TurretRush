using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public bool IsHolding { get; private set; }
    public bool IsPressedThisFrame { get; private set; }

    private bool _waitForRelease;

    private void Awake()
    {
        BlockUntilRelease();
    }

    public void BlockUntilRelease()
    {
        _waitForRelease = true;
        IsHolding = false;
        IsPressedThisFrame = false;
    }

    private void Update()
    {
        bool isClicking =
            (Mouse.current != null && Mouse.current.leftButton.isPressed) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed);

        if (_waitForRelease)
        {
            IsPressedThisFrame = false;
            IsHolding = isClicking;

            if (!isClicking)
            {
                _waitForRelease = false;
                IsHolding = false;
            }

            return;
        }

        IsPressedThisFrame = isClicking && !IsHolding;
        IsHolding = isClicking;
    }
}