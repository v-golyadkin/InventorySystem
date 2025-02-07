using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private HealthSO _healthData;

    private void Awake()
    {
        _healthData.currentHealth = _healthData.maxHealth / 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);

        if(Input.GetKeyDown(KeyCode.R))
            RestoreHealth(1);
    }

    public void TakeDamage(int value)
    {
        _healthData.currentHealth -= value;

        if(_healthData.currentHealth <= 0)
            RestoreHealth(_healthData.maxHealth);

        HealthValueChange();
    }

    public void RestoreHealth(int value)
    {
        _healthData.currentHealth += value;

        if (_healthData.currentHealth > _healthData.maxHealth)
            _healthData.currentHealth = _healthData.maxHealth;

        HealthValueChange();
    }

    public void HealthValueChange()
    {
        Debug.Log(_healthData.currentHealth);
    }
}
