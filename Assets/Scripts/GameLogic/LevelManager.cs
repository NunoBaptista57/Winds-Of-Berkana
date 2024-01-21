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
        // TODO
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
