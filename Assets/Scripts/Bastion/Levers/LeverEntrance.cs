using UnityEngine;

public class LeverEntrance : Lever
{
    public override void ChangeLeverLook()
    {
        gameObject.SetActive(false);
    }

    public override void PlayLeverAnimation()
    {
        gameObject.SetActive(false);
    }
}