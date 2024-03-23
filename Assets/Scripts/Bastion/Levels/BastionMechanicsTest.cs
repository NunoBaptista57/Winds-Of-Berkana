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
        if (GetCollectedKeys() == KeyManager.Keys.Count)
        {
            OpenSanctum();
        }
    }

    public override void OpenSanctum()
    {
        Debug.Log("Open Sanctum");
    }
}