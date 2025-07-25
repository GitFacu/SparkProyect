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
            Debug.LogWarning("No se encontr� CharacterController en el jugador.");
    }

    

    public void TakeDamage(int DanioRecibido)
    {
        currentHealth -= DanioRecibido;

        Debug.Log($"El jugador recibi� {DanioRecibido} de da�o. Vida restante: {currentHealth}");
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

   

    
}
