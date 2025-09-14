using UnityEngine;

[CreateAssetMenu(menuName = "DashBoy/PowerupData")]
public class PowerupData : ScriptableObject
{
    public string pickupName;
    public float duration = 5f;
    public float value = 1f;
}
