using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Event Sender")]
public class EventSender : ScriptableObject
{
    public event UnityAction CollectedKeyEvent;
    public event UnityAction<Transform> ReachedCheckpointEvent;
    public event UnityAction<GameState> ChangeGameStateEvent;

    public void InvokeCollectedKeyEvent()
    {
        CollectedKeyEvent.Invoke();
    }

    public void InvokeReachedCheckpointEvent(Transform checkpoint)
    {
        ReachedCheckpointEvent.Invoke(checkpoint);
    }

    public void InvokeChangeGameStateEvent(GameState gameState)
    {
        ChangeGameStateEvent.Invoke(gameState);
    }
}