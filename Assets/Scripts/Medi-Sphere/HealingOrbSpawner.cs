using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Transform spawnPoint;


    private void Start()
    {
        InvokeRepeating(nameof(SpawnOrb), 0f, spawnInterval);
    }

    private void SpawnOrb()
    {
        if (orbPrefab != null && spawnPoint != null)
        {
            Instantiate(orbPrefab, spawnPoint.position, Quaternion.identity);
        }
    }


}
