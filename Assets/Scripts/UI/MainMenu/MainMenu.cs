using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public void Play()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.Instance.StopMusic();
    }

    public void Exit()
    {
        Application.Quit();
    }
}