using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // Delete previous game data
        PlayerPrefs.DeleteAll();
    }

    public void PlayGame()
    {
        // Load overworld
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        // Quit game (not visible in Unity)
        Debug.Log("QUIT GAME PRESSED!");
        Application.Quit();
    }
}
