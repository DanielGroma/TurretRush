using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public bool IsHolding { get; private set; }
    public bool IsPressedThisFrame { get; private set; }

    private void Update()
    {
        bool isClicking = (Mouse.current != null && Mouse.current.leftButton.isPressed) ||
                          (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed);

        IsPressedThisFrame = isClicking && !IsHolding;
        IsHolding = isClicking;
    }
}