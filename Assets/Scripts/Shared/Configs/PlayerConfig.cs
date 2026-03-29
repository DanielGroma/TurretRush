using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float damage = 10f;
}