using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public float moveSpeed = 3f;
    public float maxHealth = 20f;
    public float damage = 10f;
    public float activationDistance = 15f;
}