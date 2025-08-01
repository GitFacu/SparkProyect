using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject _healthPanel;
    private Slider _healthSlider;

    private void Awake()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        
    }

    public void ChangeHealth(float value)
    {
        _healthSlider.value = value;
    }

    public void ShowPanel(bool value)
    {
        _healthPanel.SetActive(value);

    }


}
