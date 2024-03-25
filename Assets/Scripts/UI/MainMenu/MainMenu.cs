using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}