using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    void Start()
    {
        UpdateGameState(GameState.BastionState_Intro);    
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.BastionState_Intro:
                // First State
                break;

            case GameState.BastionState_Puzzle1:
                // Pick Up First Piece
                break;

            case GameState.Paused:
                // Paused
                break;

            case GameState.Victory:
                // Finished Bastion here
                break;

            case GameState.Loss:
                // Respawn Here
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(newState);
    }
}


public enum GameState
{
   BastionState_Intro,
   BastionState_Puzzle1,
   BastionState_Puzzle2,
   BastionState_Puzzle3,
   OnBoat,
   Paused,
   Victory,
   Loss
}
