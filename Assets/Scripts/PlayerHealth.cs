using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamage
{
    public static event Action OnDead;
    //[SerializeField] private HUDController hud; // ? Drag en el Inspector
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private AudioClip _hitClip;

    public int CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = value; }
    }

    private CharacterController charController;

    void Awake()
    {
        currentHealth = maxHealth;
        charController = GetComponent<CharacterController>();
        if (charController == null)
            Debug.LogWarning("No se encontró CharacterController en el jugador.");
    }

    

    public void TakeDamage(int DanioRecibido)
    {
        currentHealth -= DanioRecibido;

        SoundManager.Instance.PlaySound(_hitClip);

        Debug.Log($"El jugador recibió {DanioRecibido} de daño. Vida restante: {currentHealth}");
        if (currentHealth <= 0) Die();
    }
    public void Die()
    {
        Debug.Log("El jugador murio");
        
        if (charController != null) charController.enabled = false;
        transform.Rotate(0f, 90f, 0f);
        
        OnDead?.Invoke();
        Time.timeScale = 0f;
    }

    public void Heal(int amount)//
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Healed! Current health: " + currentHealth);
    }



}
