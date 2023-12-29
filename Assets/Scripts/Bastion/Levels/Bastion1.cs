using UnityEngine;

public class Bastion1 : BastionManager
{
    public override void ActivateLever(string lever)
    {
        throw new System.NotImplementedException();
    }

    public override void CollectKey(string key)
    {
        throw new System.NotImplementedException();
    }

    public override void OpenSanctum()
    {
        throw new System.NotImplementedException();
    }

    public enum BastionState
    {
        BastionState_Intro,
        BastionState_Puzzle1,
        WindTunnel,
        BastionState_Puzzle2,
        BastionState_Puzzle3,
        BastionState_Ending,
        Boat
    }
}
