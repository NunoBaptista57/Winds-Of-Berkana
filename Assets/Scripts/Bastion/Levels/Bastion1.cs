using UnityEngine;

public class Bastion1 : BastionManager
{
    [SerializeField] private Key _keyToOpenEntrance;
    [SerializeField] private Door _doorToSubground;
    [SerializeField] private Door _doorToTower;
    [SerializeField] private Lever _leverToFirstKey;
    [SerializeField] private Key _keyNest;
    [SerializeField] private Door _lowerDoorRightTower;
    [SerializeField] private Lever _leverNest;
    [SerializeField] private Door _doorWindTunnel;
    [SerializeField] private Door _doorLeftTower;
    [SerializeField] private GameObject _sanctumWindTunnel;

    public override void ActivateLever(Lever lever)
    {
        Debug.Log("Lever Event");
        if (lever == _leverToFirstKey)
        {
            _doorToTower.Open();
        }
        else if (lever == _leverNest)
        {
            _doorWindTunnel.Open();
            _doorLeftTower.Open();
        }
    }

    public override void CollectKey(Key key)
    {
        Debug.Log("Key Event");
        if (key == _keyNest)
        {
            _lowerDoorRightTower.Open();
        }
    }

    public override void OpenSanctum()
    {
        Debug.Log("Sanctum Event");
        _sanctumWindTunnel.SetActive(true);
    }

    public override void PlaceKey(Key key)
    {
        Debug.Log("Place Key Event");
        Debug.Log(key);
        if (key == _keyToOpenEntrance)
        {
            Debug.Log("Open subground");
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
