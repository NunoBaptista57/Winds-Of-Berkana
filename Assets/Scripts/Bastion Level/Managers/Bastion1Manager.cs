using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Bastion1Manager : MonoBehaviour
{
    Transform player;
    public LevelState levelState;
    Vector3 originalCameraPosition;
    public event Action<LevelState> OnLevelStateChanged;
    private readonly List<ISavable> _toSave = new();

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
        LevelManager levelManager = ServiceLocator.instance.GetService<LevelManager>();
        Debug.Log(levelManager.State);
        if (levelManager.State == GameState.Load)
        {
            Load();
        }

        LevelManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalCameraPosition = GameObject.Find("Cameras").GetComponent<Transform>().position;
        levelManager.UpdateGameState(GameState.Play);
        player.position = ServiceLocator.instance.GetService<CheckpointManager>().CurrentCheckpoint;

        foreach (Transform child in transform)
        {
            _toSave.Add(child.GetComponent<ISavable>());
        }

        _toSave.Add(ServiceLocator.instance.GetService<SanctumEntrance>());
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

            case GameState.Save:
                Save();
                break;

            case GameState.Load:
                Load();
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

    private void Save()
    {
        SaveFile saveFile = new();

        foreach (ISavable savable in _toSave)
        {
            saveFile = savable.Save(saveFile);
        }
        saveFile.LevelState = levelState;
        SaveSystem.Save(saveFile);
    }

    private void Load()
    {
        SaveFile saveFile = SaveSystem.Load();

        foreach (ISavable savable in _toSave)
        {
            savable.Load(saveFile);
        }

        player.position = saveFile.Checkpoint;
        UpdateLevelState(saveFile.LevelState);
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