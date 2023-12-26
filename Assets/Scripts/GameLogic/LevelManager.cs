using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;
    [SerializeField] private GameState _state;

    public void UpdateGameState(GameState newState)
    {
        GameState PreviousState = _state;

        if (PreviousState == GameState.Paused)
        {
            Time.timeScale = 1;
        }

        switch (newState)
        {
            case GameState.Paused:
                if (PreviousState == GameState.Paused)
                {
                    newState = GameState.Play;
                }
                else
                {
                    Time.timeScale = 0;
                }
                break;

            case GameState.Victory:
                // Transition Between modes
                break;

            case GameState.Remake:
                RestartCurrentScene();
                break;

            case GameState.Load:
                ServiceLocator.Instance.GetService<SaveSystem>().Load();
                break;

            case GameState.Save:
                ServiceLocator.Instance.GetService<SaveSystem>().Save();
                break;

            default:
                break;

        }

        _state = newState;
        OnGameStateChanged?.Invoke(_state);
    }


    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


public enum GameState
{
    Play,
    Paused,
    Save,
    Load,
    Victory,
    Death,
    Respawn,
    Remake
}
