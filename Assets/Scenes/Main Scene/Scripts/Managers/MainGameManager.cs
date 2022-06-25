using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager Instance;

    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
                            
        DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Play:
                // Respawn Character according to the level state

                break;
            case GameState.Paused:
                // Paused, already being handled
                break;

            case GameState.Victory:
                // Transition Between modes
                break;

            case GameState.Respawn:
                // Transition Between modes
                break;

            case GameState.Death:
                break;

            case GameState.Remake:
                this.RestartCurrentScene();
                UpdateGameState(GameState.Play);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(newState);
    }

   

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuiGame()
    {
        Application.Quit();
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
