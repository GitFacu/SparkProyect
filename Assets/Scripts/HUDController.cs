using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider _barraVida;
    public Slider _barraCarga;
    [SerializeField] private TextMeshProUGUI _textoMonedas;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _panelPerder;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerHealth _playerHealth;

    [Header("Valores simulados")]
    public int monedas = 0;
    private float carga = 0f;

    void Start()
    {
        
        _barraCarga.minValue = 1f;
        _barraCarga.maxValue = 2f;
        _barraCarga.value = 0f;
        _barraCarga.gameObject.SetActive(false);
        _crosshair.gameObject.SetActive(false);
        _panelPerder.gameObject.SetActive(false);
       
    }

    void Update()
    {
        ActualizarHUD();

        // Simulación de daño y recolección
       
        if (Input.GetKeyDown(KeyCode.M)) monedas += 1;

        if (Input.GetMouseButtonDown(1)) // clic derecho presionado
            _crosshair.SetActive(true);

        if (Input.GetMouseButtonUp(1))   // clic derecho soltado
            _crosshair.SetActive(false);
    }
    public void ActualizarVida(int vida)
    {
        _barraVida.value = vida;
    }
    public void ActualizarMonedas()
    {
        int coins = _player.CoinAmount;
        _textoMonedas.text = "Monedas: " + coins;
    }
    // Llamar para encender la barra antes de empezar a cargar
    public void ShowChargeBar()
    {
        _barraCarga.value = 0f;
        _barraCarga.gameObject.SetActive(true);
    }

    // Llamar mientras cargas para actualizar el llenado (0–1)
    public void UpdateChargeBar(float normalized)
    {
        float porcentaje = Mathf.Clamp01(normalized) * _barraCarga.maxValue;
        _barraCarga.value = porcentaje;
    }

    // Llamar al soltar el botón de carga
    public void HideChargeBar()
    {
        _barraCarga.gameObject.SetActive(false);
        _barraCarga.value = 0f;
    }
    void ActualizarHUD()
    {
        if (_playerHealth != null)
        {
              ActualizarVida(_playerHealth.CurrentHealth);
        }
      
        _barraCarga.value = carga;
        _textoMonedas.text = "Monedas: " + _player.CoinAmount;
    }

    private void ShowDeathScreen(GameObject player)
    {
        _panelPerder.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += ShowDeathScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= ShowDeathScreen;
    }
}
