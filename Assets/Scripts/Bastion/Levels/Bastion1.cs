using UnityEngine;

public class Bastion1 : BastionManager
{
    [SerializeField] private Key _keyToOpenEntrance;
    [SerializeField] private Door _doorToSubground;
    [SerializeField] private Door _doorToTower;
    [SerializeField] private Lever _leverToFirstKey;

    public override void ActivateLever(Lever lever)
    {
        Debug.Log("Lever Event");
        if (lever == _leverToFirstKey)
        {
            _doorToTower.Open();
        }
    }

    public override void CollectKey(Key key)
    {
        Debug.Log("Key Event");
    }

    public override void OpenSanctum()
    {
        Debug.Log("Sanctum Event");
        throw new System.NotImplementedException();
    }

    public override void PlaceKey(Key key)
    {
        Debug.Log("Place Key Event");
        if (key == _keyToOpenEntrance)
        {
            _doorToSubground.Open();
        }
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
