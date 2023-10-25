using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        GameState PreviousState = State;

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
                ServiceLocator.instance.GetService<SaveSystem>().Load();
                break;

            case GameState.Save:
                ServiceLocator.instance.GetService<SaveSystem>().Save();
                break;

            default:
                break;

        }

        State = newState;
        OnGameStateChanged?.Invoke(State);
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
