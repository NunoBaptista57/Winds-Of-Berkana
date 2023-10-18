using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public List<ILever> Levers = new();

    public void AddLever(ILever lever)
    {
        Levers.Add(lever);
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
}