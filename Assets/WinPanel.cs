using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    

    private void OnEnable()
    {
        BugTower.OnDead += OnShowPanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        _winPanel.SetActive(false);
    }

    private void OnDisable()
    {
        BugTower.OnDead -= OnShowPanel;
    }

    private void OnShowPanel()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 1f;
        

    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel 2");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicial");
    }



}
