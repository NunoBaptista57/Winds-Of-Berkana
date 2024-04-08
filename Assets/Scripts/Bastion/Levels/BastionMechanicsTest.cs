using UnityEngine;

public class BastionMechanicsTest : BastionManager
{
    public override void ActivateLever(Lever lever)
    {
        throw new System.NotImplementedException();
    }

    public override void CollectKey(Key key)
    {
        Debug.Log("Collected " + key);
    }

    public override void OpenSanctum()
    {
        Debug.Log("Open Sanctum");
        ServiceLocator.Instance.GetService<LevelManager>().GoToNextLevel();
    }

    public override void PlaceKey(Key key)
    {
        throw new System.NotImplementedException();
    }
}