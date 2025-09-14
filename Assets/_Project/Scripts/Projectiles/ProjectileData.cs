using UnityEngine;

[CreateAssetMenu(menuName = "DashBoy/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float speed = 8f;
    public int damage = 1;
    public float lifeTime = 4f;
}
