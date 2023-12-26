using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Bastion1Manager : MonoBehaviour, ISavable
{
    public LevelState LevelState;
    public event Action<LevelState> OnLevelStateChanged;
    private Vector3 _originalCameraPosition;
    private Transform _player;
    private LevelManager _levelManager;

    public SaveFile Save(SaveFile saveFile)
    {
        saveFile.LevelState = LevelState;
        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        _player.position = saveFile.Checkpoint;
        UpdateLevelState(saveFile.LevelState);
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

    private void Awake()
    {
        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        _levelManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        _levelManager.UpdateGameState(GameState.Play);

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _player.position = ServiceLocator.Instance.GetService<CheckpointManager>().CurrentCheckpoint;

        _originalCameraPosition = GameObject.Find("Cameras").GetComponent<Transform>().position;
    }

    private void UpdateLevelState(LevelState newState)
    {
        LevelState = newState;
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
                _player.position = ServiceLocator.Instance.GetService<CheckpointManager>().CurrentCheckpoint;
                _player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameObject.Find("Cameras").GetComponent<Transform>().position = _originalCameraPosition;
                ServiceLocator.Instance.GetService<LevelManager>().UpdateGameState(GameState.Play);
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

        ServiceLocator.Instance.GetService<LevelManager>().UpdateGameState(GameState.Respawn);
    }

    private void OnDisable()
    {
        _levelManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
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