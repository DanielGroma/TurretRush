using UnityEngine;

[CreateAssetMenu(menuName = "Configs/TurretConfig")]
public class TurretConfig : ScriptableObject
{
    public float fireRate = 0.5f;
    public float rotationSpeed = 120f;
    public float maxAngle = 45f;
}
