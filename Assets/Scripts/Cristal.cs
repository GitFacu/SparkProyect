using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : MonoBehaviour, IDamage
{
    [SerializeField] private float _elementHP = 100;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPose;

    public void Die()
    {
        Destroy(gameObject);
    }

    private void CreateObject()
    {
        GameObject Obj = Instantiate(_prefab,_spawnPose.position,_prefab.transform.rotation);
    }

    public void TakeDamage(int danioRecibido)
    {
        _elementHP -= danioRecibido;
        if (_elementHP <= 0)
        {
            CreateObject();
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
