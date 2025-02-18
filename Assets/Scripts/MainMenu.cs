using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GamePlay()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Rules()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void AboutUs()
    {
        SceneManager.LoadSceneAsync(3);
    }
}
