using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BastionManager : MonoBehaviour, IManager
{
    [SerializeField] private EventSender _eventSender;

    private Transform _startPosition;
    private GameObject _player;
    private List<IDoor> _doors;
    private List<Key> _keys;

    private Transform _currentCheckpoint;

    private void Awake()
    {
        _startPosition = transform.Find("StartPosition");
        _player = transform.Find("Player").gameObject;
        _doors = new();
        _keys = new();

        foreach (IDoor door in transform.Find("Doors").GetComponentsInChildren<IDoor>())
        {
            _doors.Append(door);
        }

        foreach (Key key in transform.Find("Keys").GetComponentsInChildren<Key>())
        {
            _keys.Append(key);
        }
    }

    private void Start()
    {
        Reset();
        _player.transform.position = _currentCheckpoint.position;
    }

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

    private void UpdateCheckpoint(Transform checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }

    private void OnEnable()
    {
        _eventSender.CollectedKeyEvent += UpdateDoors;
        _eventSender.ReachedCheckpointEvent += UpdateCheckpoint;
    }

    private void OnDisable()
    {
        _eventSender.CollectedKeyEvent -= UpdateDoors;
        _eventSender.ReachedCheckpointEvent -= UpdateCheckpoint;
    }

    public void Save()
    {
        Debug.Log("Save");
    }

    public void Load()
    {
        Debug.Log("Load");
    }

    public void Reset()
    {
        _currentCheckpoint = _startPosition;
        _eventSender.InvokeChangeGameStateEvent(GameState.Play);
    }
}
