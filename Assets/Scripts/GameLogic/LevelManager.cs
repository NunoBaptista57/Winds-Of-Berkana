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

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    public void UpdateGameState(GameState newState)
    {
        var PreviousState = State;
        State = newState;

        switch (newState)
        {
            case GameState.Paused:
                if (PreviousState == GameState.Paused)
                    State = GameState.Play;
                break;

            case GameState.Victory:
                // Transition Between modes
                break;

            case GameState.Remake:
                RestartCurrentScene();
                UpdateGameState(GameState.Play);
                break;

            default:
                break;

        }

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
    Victory,
    Death,
    Respawn,
    Remake
}
