using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour, ISavable
{
    public List<Lever> Levers = new();

    public SaveFile Save(SaveFile saveFile)
    {
        bool[] levers = new bool[Levers.Count];

        for (int i = 0; i < Levers.Count; i++)
        {
            levers[i] = Levers[i].IsActivated;
        }

        saveFile.Levers = levers;

        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        for (int i = 0; i < Levers.Count; i++)
        {
            Levers[i].IsActivated = true;
            Levers[i].DoorOpened = true;
        }
    }

    public void UpdateLevers()
    {
        foreach (Lever lever in Levers)
        {
            if (!lever.DoorOpened && lever.IsActivated)
            {
                lever.DoorOpened = true;
                ServiceLocator.Instance.GetService<Bastion1Manager>().ActivateLever(lever.ID);
            }
        }
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Lever lever = child.gameObject.GetComponent<Lever>();
            lever.LeverManager = this;
            Levers.Add(lever);
        }
    }
}