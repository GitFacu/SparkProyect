using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    private int _currentHealth;
    [SerializeField] GameObject _deathPrefab;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {_currentHealth}");
        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Un enemigo murio");
        if (_deathPrefab != null)
            Instantiate(_deathPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

