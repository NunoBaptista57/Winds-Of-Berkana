using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject _content;

    public void Resume()
    {
        ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Paused);
    }

    public void Restart()
    {
        ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Remake);
    }

    private void Start()
    {
        _content = transform.GetChild(0).gameObject;
        LevelManager.OnGameStateChanged += Pause;
    }

    private void Pause(GameState gameState)
    {
        if (gameState == GameState.Play)
        {
            _content.SetActive(false);
        }
        else if (gameState == GameState.Paused)
        {
            _content.SetActive(true);
        }
    }
}