using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Scriptable Objects/HealthData")]
public class HealthSO : ScriptableObject
{
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
}
