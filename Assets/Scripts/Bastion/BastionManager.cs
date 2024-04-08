using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public abstract class BastionManager : MonoBehaviour
{
    protected KeyManager KeyManager;
    protected SanctumEntrance SanctumEntrance;
    protected LeverManager LeverManager;
    protected DoorManager DoorManager;
    protected VitralPuzzleManager VitralPuzzleManager;
    public abstract void CollectKey(Key key);
    public abstract void PlaceKey(Key key);
    public abstract void ActivateLever(Lever lever);
    public abstract void OpenSanctum();

    public List<Key> GetCollectedKeys()
    {
        if (KeyManager == null)
        {
            throw new NullReferenceException("KeyManager doesn't exist");
        };

        return KeyManager.Keys.Where(key => key.Collected).ToList();
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

    private void Start()
    {
        if (KeyManager != null)
        {
            KeyManager.CollectedKeyEvent.AddListener(CollectKey);
        }
        if (SanctumEntrance != null)
        {
            SanctumEntrance.OpenSanctumEvent.AddListener(OpenSanctum);
            SanctumEntrance.PlaceKeyEvent.AddListener(PlaceKey);
        }
        if (LeverManager != null)
        {
            LeverManager.LeverActivatedEvent.AddListener(ActivateLever);
        }
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