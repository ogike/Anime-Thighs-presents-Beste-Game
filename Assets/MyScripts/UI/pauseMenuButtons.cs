using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuButtons : MonoBehaviour
{
    // i had to make this script for the buttons seperately because without this
    // i had the press the ESC multiple times for the menu 2 show up 
    // i have ho clue why le fuck


    // spagoot cooked by ReRo OwO 

    public GameObject PauseMenu;

    public static bool isPaused;

    public void PauseTheGame()
    {
        PauseMenu.SetActive(true);
         isPaused = true;
        Time.timeScale = 0f;
       
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        
        Time.timeScale = 1;

        isPaused = false;
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}