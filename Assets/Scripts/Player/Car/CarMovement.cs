using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private bool canMove = false;

    public void StartMoving() => canMove = true;
    public void StopMoving() => canMove = false;

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(transform.forward * _moveSpeed * Time.deltaTime, Space.World);
        }
    }
}