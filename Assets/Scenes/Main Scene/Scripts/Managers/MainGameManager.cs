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

    [SerializeField] private EventSender _eventSender;

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
            case GameState.Play:
                // Respawn Character according to the level state

                break;
            case GameState.Paused:
                
                if(PreviousState == GameState.Paused)
                    State = GameState.Play;
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
                RestartCurrentScene();
                UpdateGameState(GameState.Play);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        OnGameStateChanged?.Invoke(State);
    }

   

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuiGame()
    {
        Application.Quit();
    }
   
    private void OnEnable()
    {
        _eventSender.ChangeGameStateEvent += UpdateGameState;
    }

    private void OnDisable()
    {
        _eventSender.ChangeGameStateEvent -= UpdateGameState;
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
