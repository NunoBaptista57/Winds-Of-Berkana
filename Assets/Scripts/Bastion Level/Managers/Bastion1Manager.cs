using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bastion1Manager : MonoBehaviour
{
    Transform player;
    public LevelState levelState;
    Vector3 originalCameraPosition;
    public event Action<LevelState> OnLevelStateChanged;
    private List<IManager> _managers = new();
    private SaveFile _saveFile = new();

    public void Save()
    {
        foreach (IManager manager in _managers)
        {
            _saveFile = manager.Save(_saveFile);
        }

        _saveFile.PlacedKeys = ServiceLocator.instance.GetService<SanctumEntrance>().PlacedKeys;
        SaveSystem.Save(_saveFile);
    }

    public void Load()
    {
        _saveFile = SaveSystem.Load();

        foreach (IManager manager in _managers)
        {
            manager.Load(_saveFile);
        }

        SanctumEntrance sanctumEntrance = ServiceLocator.instance.GetService<SanctumEntrance>();

        sanctumEntrance.PlacedKeys = _saveFile.PlacedKeys;
        sanctumEntrance.PlaceKeys();

        UpdateLevelState(_saveFile.LevelState);
    }

    public void PickUpKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 1:
                UpdateLevelState(LevelState.BastionState_Puzzle1);
                break;

            case 2:
                UpdateLevelState(LevelState.BastionState_Puzzle2);
                break;

            case 3:
                UpdateLevelState(LevelState.BastionState_Puzzle3);
                break;

            default:
                break;
        }
    }

    public void ActivateLever(int leverNumber)
    {
        switch (leverNumber)
        {
            case 0:
                UpdateLevelState(LevelState.WindTunnel);
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        LevelManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalCameraPosition = GameObject.Find("Cameras").GetComponent<Transform>().position;
        ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Play);
        player.position = ServiceLocator.instance.GetService<CheckpointManager>().CurrentCheckpoint;

        foreach (Transform child in transform)
        {
            _managers.Add(child.GetComponent<IManager>());
        }
    }

    private void UpdateLevelState(LevelState newState)
    {
        levelState = newState;
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
                player.position = ServiceLocator.instance.GetService<CheckpointManager>().CurrentCheckpoint;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameObject.Find("Cameras").GetComponent<Transform>().position = originalCameraPosition;
                ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Play);
                break;
        }

    }

    private async void HandleDeath()
    {
        await System.Threading.Tasks.Task.Delay(500);

        if (GameObject.Find("Death Camera"))
        {
            GameObject.Find("Death Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 15;

            await System.Threading.Tasks.Task.Delay(1000);

            GameObject.Find("Death Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 8;
        }

        ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Respawn);
    }

}

public enum LevelState
{
    BastionState_Intro,
    BastionState_Puzzle1,
    WindTunnel,
    BastionState_Puzzle2,
    BastionState_Puzzle3,
    BastionState_Ending,
    Boat
}