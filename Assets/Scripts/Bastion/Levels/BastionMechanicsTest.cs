using UnityEngine;

public class BastionMechanicsTest : BastionManager
{
    public override void ActivateLever(string lever)
    {
        throw new System.NotImplementedException();
    }

    public override void CollectKey(string key)
    {
        Debug.Log("Collected " + key);
    }

    public override void OpenSanctum()
    {
        Debug.Log("Open Sanctum");
        ServiceLocator.Instance.GetService<LevelManager>().GoToNextLevel();
    }
}