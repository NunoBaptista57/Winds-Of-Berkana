using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager Instance;

    public GameState State;
    public LevelState levelState;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<LevelState> OnLevelStateChanged;


    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        UpdateGameState(GameState.Play);
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
                HandleDeath();
                break;

            case GameState.Remake:
                this.RestartCurrentScene();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void UpdateLevelState(LevelState newState)
    {
        levelState = newState;

        switch (newState)
        {
            case LevelState.BastionState_Intro:
                // Play mode 
                break;
            case LevelState.BastionState_Puzzle1:
                // Paused, already being handled
                break;

            case LevelState.BastionState_Ending:
                // Finished Bastion here
                break;

            case LevelState.Boat:
                // Finished Bastion here
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnLevelStateChanged?.Invoke(newState);
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private async void HandleDeath()
    {
        await System.Threading.Tasks.Task.Delay(500);

        if (GameObject.Find("Death Camera"))
        {
            GameObject.Find("Death Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 15;

            await System.Threading.Tasks.Task.Delay(2000);

            GameObject.Find("Death Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 8;
        }

        UpdateGameState(GameState.Respawn);
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

public enum LevelState
{
   BastionState_Intro,
   BastionState_Puzzle1,
   BastionState_Puzzle2,
   BastionState_Puzzle3,
   BastionState_Ending,
   Boat
}
