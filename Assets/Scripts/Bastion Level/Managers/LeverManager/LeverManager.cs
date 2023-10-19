using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour, IManager
{
    public List<ILever> Levers = new();

    public SaveFile Save(SaveFile saveFile)
    {
        bool[] levers = new bool[Levers.Count];

        for (int i = 0; i < Levers.Count; i++)
        {
            levers[i] = Levers[i].IsActivated();
        }

        saveFile.Levers = levers;

        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        for (int i = 0; i < Levers.Count; i++)
        {
            Levers[i].SetActivated(saveFile.Levers[i]);
        }
    }

    public void UpdateLevers()
    {
        foreach (ILever lever in Levers)
        {
            if (!lever.IsActivated() && lever.ToActivate())
            {
                lever.SetActivated(true);
                ServiceLocator.instance.GetService<Bastion1Manager>().ActivateLever(lever.GetID());
            }
        }
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Levers.Add(child.gameObject.GetComponent<ILever>());
        }
    }
}