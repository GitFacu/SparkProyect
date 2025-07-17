using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamage
{
    [SerializeField] protected int _health;
    [SerializeField] protected bool estado;
    [SerializeField] protected GameObject _mesh;

    public virtual void Die()
    {
        _mesh.SetActive(false);
    }

    public virtual void TakeDamage(int danioRecibido)
    {
        _health -= danioRecibido;

        if (_health <= 0) Die();
    }
}
