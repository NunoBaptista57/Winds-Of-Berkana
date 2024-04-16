using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("Death");
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Debug.Log("Spawning Player...");
        _playerManager.Spawn(_checkpointManager.CurrentCheckpoint.RespawnPosition);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            _gameState = GameState.Paused;
            _playerManager.CanMoveCamera = false;
            Time.timeScale = 0f;
            _playerManager.SetCanMove(false);
        }
        else
        {
            _gameState = GameState.Play;
            _playerManager.CanMoveCamera = true;
            Time.timeScale = 1f;
            _playerManager.SetCanMove(true);
        }
    }

    public Level Save()
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
            level.Bastion = _bastionManager.SaveBastion();
        }

        // TODO boatLevel
        return level;
    }

    public void Load()
    {
        _gameState = GameState.Load;
        // TODO
    }

    public void Restart()
    {
        _gameState = GameState.Remake;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void GoToNextLevel()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (activeSceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            QuitToMenu();
            return;
        }
        SceneManager.LoadScene(activeSceneIndex + 1);
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
        Cursor.visible = false;
    }

}

[Serializable]
public struct Level
{
    public Bastion Bastion;
    // TODO BoatLevel BoatLevel;
    public Vector2 Checkpoint;
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