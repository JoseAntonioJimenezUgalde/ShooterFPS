using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Comenzar a jugar, cargar scene de juego
    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Salir del juego
    
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit(); 
    }
}
