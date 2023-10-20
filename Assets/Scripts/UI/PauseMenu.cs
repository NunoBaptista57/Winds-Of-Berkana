using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject _content;
    private LevelManager levelManager;

    public void Resume()
    {
        levelManager.UpdateGameState(GameState.Play);
    }

    public void Load()
    {
        levelManager.UpdateGameState(GameState.Load);
        levelManager.UpdateGameState(GameState.Play);
    }

    public void Save()
    {
        levelManager.UpdateGameState(GameState.Save);
        levelManager.UpdateGameState(GameState.Paused);
    }

    public void Restart()
    {
        levelManager.UpdateGameState(GameState.Remake);
    }

    private void Start()
    {
        _content = transform.GetChild(0).gameObject;
        LevelManager.OnGameStateChanged += Pause;
        levelManager = ServiceLocator.instance.GetService<LevelManager>();
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

    private void OnDestroy()
    {
        LevelManager.OnGameStateChanged -= Pause;
    }
}