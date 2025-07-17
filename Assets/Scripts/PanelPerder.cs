using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelPerder : MonoBehaviour
{
  
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
