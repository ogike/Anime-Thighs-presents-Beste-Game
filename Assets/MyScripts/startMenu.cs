using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class startMenu : MonoBehaviour
{
    //start menu of the game
    //by ReRo, it aint much but its honest work

    //Application quitting does work I swear, it just that it only works in the actual game, not the editor
    //I spent way more time realizing this than Im willing to admit lol
    public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
