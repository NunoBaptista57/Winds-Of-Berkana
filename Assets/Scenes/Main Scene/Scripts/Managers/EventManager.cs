using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : ScriptableObject
{
    public event UnityAction CollectedKeyEvent;
    public event UnityAction<Checkpoint> ReachedCheckpointEvent;

    public void InvokeCollectedKeyEvent()
    {
        CollectedKeyEvent.Invoke();
    }

    public void InvokeReachedCheckpointEvent(Checkpoint checkpoint)
    {
        ReachedCheckpointEvent.Invoke(checkpoint);
    }
}