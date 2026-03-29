using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
     public float speed = 20f;
     public float damage = 10f;
     public float lifeTime = 5f;
}
