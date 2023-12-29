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
        Bastion bastionSave = new()
        {
            Levers = LeverManager.SaveLevers(),
            Doors = DoorManager.SaveDoors(),
            Keys = KeyManager.SaveKeys(),
            PlacedKeys = SanctumEntrance.PlacedKeys,
            VitralIsComplete = VitralPuzzleManager.IsComplete
        };
        return bastionSave;
    }

    public void LoadBastion(Bastion bastion)
    {
        LeverManager.LoadLevers(bastion.Levers);
        DoorManager.LoadDoors(bastion.Doors);
        KeyManager.LoadKeys(bastion.Keys);
        SanctumEntrance.LoadAltars(bastion.PlacedKeys);
        VitralPuzzleManager.IsComplete = bastion.VitralIsComplete;
        VitralPuzzleManager.CompletedVitral();
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
        if (SanctumEntrance.KeysToOpen != 0)
        {
            SanctumEntrance.KeysToOpen = KeyManager.Keys.Count;
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