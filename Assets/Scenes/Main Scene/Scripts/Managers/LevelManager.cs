using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private EventManager _eventManager;
    private Checkpoint _currentCheckpoint;
    private List<Key> _keys;
    private List<IDoor> _doors;

    private void UpdateDoors()
    {
        foreach (IDoor door in _doors)
        {
            if (door.CanOpen() && !door.IsOpen())
            {
                door.Open();
            }
        }
    }

    private void UpdateCheckpoint(Checkpoint checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }

    private void OnEnable()
    {
        _eventManager.CollectedKeyEvent += UpdateDoors;
        _eventManager.ReachedCheckpointEvent += UpdateCheckpoint;
    }

    private void OnDisable()
    {
        _eventManager.CollectedKeyEvent -= UpdateDoors;
        _eventManager.ReachedCheckpointEvent -= UpdateCheckpoint;
    }
}
