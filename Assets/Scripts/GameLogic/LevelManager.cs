using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private CheckpointManager _checkpointManager;
    private PlayerManager _playerManager;
    private BastionManager _bastionManager;
    // TODO private BoatLevelManager _boatLevelManager;
    private DeathZone _deathZone;
    private GameState _gameState;

    public void Death()
    {
        // TODO
    }

    public void SpawnPlayer()
    {
        _playerManager.Spawn(_checkpointManager.CurrentCheckpoint.transform);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            _gameState = GameState.Paused;
            // TODO
        }
        else
        {
            _gameState = GameState.Play;
            // TODO
        }
    }

    public void Save()
    {
        _gameState = GameState.Save;
        Level level = new();

        if (_checkpointManager != null)
        {
            Vector3 checkpoint = _checkpointManager.CurrentCheckpoint.transform.position;
            level.Checkpoint = checkpoint;
        }

        if (_bastionManager != null)
        {
            Bastion bastion = _bastionManager.SaveBastion();
        }
        // TODO boatLevel
    }

    public void Load()
    {
        _gameState = GameState.Load;
        // TODO
    }

    public void Restart()
    {
        _gameState = GameState.Remake;
        // TODO
    }

    public void Quit()
    {
        // TODO
    }

    private void Awake()
    {
        _checkpointManager = GetComponentInChildren<CheckpointManager>();
        _bastionManager = GetComponentInChildren<BastionManager>();
        _deathZone = GetComponentInChildren<DeathZone>();
        // TODO _boatLevelManager = GetComponentInChildren<BoatLevelManager>();
    }

    private void Start()
    {
        _playerManager = ServiceLocator.Instance.GetService<PlayerManager>();

        if (_checkpointManager != null && _checkpointManager.CurrentCheckpoint != null)
        {
            SpawnPlayer();
        }
    }

    public enum GameState
    {
        Play,
        Paused,
        Save,
        Load,
        Victory,
        Death,
        Respawn,
        Remake
    }
}


[Serializable]
public struct Level
{
    public Bastion Bastion;
    // TODO BoatLevel BoatLevel;
    public Vector2 Checkpoint;
}