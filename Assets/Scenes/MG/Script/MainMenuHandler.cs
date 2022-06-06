using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    //[SerializeField] private GameObject _pasueButton;
    public bool paused = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

   public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    } 

    public void HandlePause()
    {
        if (paused)
            Resume();
        else Pause();
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }


}
