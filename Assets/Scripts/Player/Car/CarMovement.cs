using UnityEngine;
using Zenject;

public class CarMovement : MonoBehaviour
{
    private PlayerConfig _config;

    private bool canMove = false;

    [Inject]
    public void Construct(PlayerConfig playerConfig)
    {
        _config = playerConfig;
    }

    public void StartMoving() => canMove = true;
    public void StopMoving() => canMove = false;

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(transform.forward * _config.moveSpeed * Time.deltaTime, Space.World);
        }
    }
}