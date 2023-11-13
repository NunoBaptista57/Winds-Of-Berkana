using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private GameObject _content;
    private readonly LevelManager levelManager;

    public void Pause(InputAction.CallbackContext context)
    {
        if (_content.activeSelf)
        {
            Resume();
            return;
        }
        _content.SetActive(true);
        levelManager.UpdateGameState(GameState.Paused);
    }
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
    }
}