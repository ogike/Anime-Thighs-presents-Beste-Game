using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    // the basic version of pause menu, use ESC to activate
    // spagoot cooked by ReRo OwO 
    // only current issue is that u can still forog and (kinf of, not really) shoot when game is paused
    // the code responsible for shooting will need to be tweaked slightly for the latter

    public GameObject PauseMenu;

    public static bool isPaused;
    void Start()
    {
        PauseMenu.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
