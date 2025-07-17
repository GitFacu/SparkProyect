using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyElement : MonoBehaviour, IDamage
{
    [SerializeField] private float _health = 100;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int danioRecibido)
    {
        _health -= danioRecibido;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            TakeDamage(50);
            
        }
    }
}
