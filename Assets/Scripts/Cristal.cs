using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : MonoBehaviour, IDamage
{
    [SerializeField] private float elementHP = 100;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPose;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void CreateObject()
    {
        GameObject Obj = Instantiate(_prefab,_spawnPose.position,_prefab.transform.rotation);
    }

    public void TakeDamage(int danioRecibido)
    {
        elementHP -= danioRecibido;
        if (elementHP <= 0)
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
