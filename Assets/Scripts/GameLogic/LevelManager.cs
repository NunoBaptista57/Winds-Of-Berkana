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
        GameState PreviousState = State;

        switch (newState)
        {
            case GameState.Paused:
                if (PreviousState == GameState.Paused)
                {
                    newState = GameState.Play;
                    Time.timeScale = 1;
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
                UpdateGameState(GameState.Play);
                break;

            default:
                break;

        }

        State = newState;
        Debug.Log(State);
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
