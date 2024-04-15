using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverManager : MonoBehaviour
{
    public List<Lever> Levers = new();
    public UnityEvent<Lever> LeverActivatedEvent;

    public bool[] SaveLevers()
    {
        bool[] levers = new bool[Levers.Count];

        for (int i = 0; i < Levers.Count; i++)
        {
            levers[i] = Levers[i].IsActivated;
        }

        return levers;
    }

    public void LoadLevers(bool[] levers)
    {
        for (int i = 0; i < Levers.Count; i++)
        {
            if (levers[i])
            {
                Levers[i].IsActivated = true;
                Levers[i].ChangeLeverLook();
            }
        }
    }

    public void ActivateLever(Lever lever)
    {
        LeverActivatedEvent.Invoke(lever);
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Lever lever))
            {
                Levers.Add(lever);
            }
        }
    }
}