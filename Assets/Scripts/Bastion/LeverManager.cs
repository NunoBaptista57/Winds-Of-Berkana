using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public List<Lever> Levers = new();
    private BastionManager _bastionManager;

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

    public void ActivateLever(string lever)
    {
        _bastionManager.ActivateLever(lever);
    }

    private void Start()
    {
        if (_bastionManager == null && transform.parent.TryGetComponent(out BastionManager bastionManager))
        {
            _bastionManager = bastionManager;
        }
        else if (_bastionManager == null)
        {
            Debug.Log("KeyManager: BastionManager not found. Using ServiceLocator...");
            _bastionManager = ServiceLocator.Instance.GetService<BastionManager>();
        }


        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Lever lever))
            {
                Levers.Add(lever);
            }
        }
    }
}