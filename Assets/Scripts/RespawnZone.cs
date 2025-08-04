using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnZone : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    private Collider _collider;
    [SerializeField] private Transform _respawnPoint;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController player = other.GetComponent<CharacterController>();

        if (player != null)
        {
            player.enabled = false;
            player.transform.position = _respawnPoint.position;
            player.enabled = true;
            //_event?.Invoke();
            Debug.Log("Respawn");
        }
    }
}
