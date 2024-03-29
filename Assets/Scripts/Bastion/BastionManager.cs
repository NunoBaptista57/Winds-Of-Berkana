using System;
using UnityEngine;

public abstract class BastionManager : MonoBehaviour
{
    protected SanctumEntrance SanctumEntrance;
    protected KeyManager KeyManager;
    protected LeverManager LeverManager;
    protected DoorManager DoorManager;
    protected VitralPuzzleManager VitralPuzzleManager;

    public abstract void CollectKey(string key);
    public abstract void ActivateLever(string lever);
    public abstract void OpenSanctum();

    public int GetCollectedKeys()
    {
        if (KeyManager == null)
        {
            throw new NullReferenceException("KeyManager doesn't exist");
        };
        int collectedKeys = 0;
        foreach (Key key in KeyManager.Keys)
        {
            if (key.Collected)
            {
                collectedKeys++;
            }
        }
        return collectedKeys;
    }

    public Bastion SaveBastion()
    {
        Bastion bastionSave = new();
        if (LeverManager != null)
        {
            bastionSave.Levers = LeverManager.SaveLevers();
        }
        if (DoorManager != null)
        {
            bastionSave.Doors = DoorManager.SaveDoors();
        }
        if (KeyManager != null)
        {
            bastionSave.Keys = KeyManager.SaveKeys();
        }
        if (SanctumEntrance != null)
        {
            bastionSave.PlacedKeys = SanctumEntrance.PlacedKeys;
        }
        if (VitralPuzzleManager != null)
        {
            bastionSave.VitralIsComplete = VitralPuzzleManager.IsComplete;
        }
        return bastionSave;
    }

    public void LoadBastion(Bastion bastion)
    {
        if (LeverManager != null)
        {
            LeverManager.LoadLevers(bastion.Levers);
        }
        if (DoorManager != null)
        {
            DoorManager.LoadDoors(bastion.Doors);
        }
        if (KeyManager != null)
        {
            KeyManager.LoadKeys(bastion.Keys);
        }
        if (SanctumEntrance != null)
        {
            SanctumEntrance.LoadAltars(bastion.PlacedKeys);
        }
        if (VitralPuzzleManager != null)
        {
            VitralPuzzleManager.IsComplete = bastion.VitralIsComplete;
            VitralPuzzleManager.CompletedVitral();
        }
    }
    private void Awake()
    {
        SanctumEntrance = GetComponentInChildren<SanctumEntrance>();
        KeyManager = GetComponentInChildren<KeyManager>();
        LeverManager = GetComponentInChildren<LeverManager>();
        DoorManager = GetComponentInChildren<DoorManager>();
    }
}

[Serializable]
public struct Bastion
{
    public bool[] Levers;
    public bool[] Doors;
    public bool[] Keys;
    public int PlacedKeys;
    public bool VitralIsComplete;
}