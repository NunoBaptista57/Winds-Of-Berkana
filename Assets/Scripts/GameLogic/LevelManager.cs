using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private CheckpointManager _checkpointManager;
    private PlayerManager _playerManager;
    private BastionManager _bastionManager;
    private InputManager _inputManager;
    // TODO private BoatLevelManager _boatLevelManager;

    public void SpawnPlayer()
    {

    }

    private void Awake()
    {
        _checkpointManager = GetComponentInChildren<CheckpointManager>();
        _playerManager = GetComponentInChildren<PlayerManager>();
        _bastionManager = GetComponentInChildren<BastionManager>();
        // TODO _boatLevelManager = GetComponentInChildren<BoatLevelManager>();
    }

    private void Start()
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
