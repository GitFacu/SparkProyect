using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider _sliderHealth;
    [SerializeField] private TextMeshProUGUI _textCoins;
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _panelLoose;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerHealth _playerHealth;
    

    void Start()
    {
        
        _crosshair.gameObject.SetActive(false);
        _panelLoose.gameObject.SetActive(false);
       
    }

    void Update()
    {
        ActualizarHUD();


        if (Input.GetMouseButtonDown(1)) // clic derecho presionado
            _crosshair.SetActive(true);

        if (Input.GetMouseButtonUp(1))   // clic derecho soltado
            _crosshair.SetActive(false);
    }
    public void ActualizarVida(int vida)
    {
        _sliderHealth.value = vida;
    }
    public void ActualizarMonedas()
    {
        int coins = _player.CoinAmount;
        _textCoins.text = "Monedas: " + coins;
    }
   
    void ActualizarHUD()
    {
        if (_playerHealth != null)
        {
              ActualizarVida(_playerHealth.CurrentHealth);
        }
      
        _textCoins.text = "Monedas: " + _player.CoinAmount;
    }

    private void ShowDeathScreen()
    {
        _panelLoose.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        PlayerHealth.OnDead += ShowDeathScreen;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDead -= ShowDeathScreen;
    }
}
