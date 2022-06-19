using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bastion1LevelManager : MonoBehaviour
{

    MainGameManager manager;
    Transform player;
    public LevelState levelState;

    Vector3 originalCameraPosition;

    public static event Action<LevelState> OnLevelStateChanged;

    [Serializable]
    public struct SpawnLocation
    {
        public LevelState state;
        public Vector3 position;
    }

    public SpawnLocation[] spawnLocations;

    void Awake()
    {
        manager = MainGameManager.Instance;
        MainGameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalCameraPosition = GameObject.Find("Cameras").GetComponent<Transform>().position;
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



    private void GameManagerOnGameStateChanged(GameState state)
    {

        switch (state)
        {
            case GameState.Death:
                HandleDeath();
                break;

            case GameState.Respawn:
                var location = Vector3.zero;
                foreach (var spawnL in spawnLocations)
                {
                    if (spawnL.state == levelState)
                    {
                        location = spawnL.position;
                        break;

                    }
                }

                player.position = location;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameObject.Find("Cameras").GetComponent<Transform>().position = originalCameraPosition;
                MainGameManager.Instance.UpdateGameState(GameState.Play);
                break;
        }
     
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

        manager.UpdateGameState(GameState.Respawn);
    }

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