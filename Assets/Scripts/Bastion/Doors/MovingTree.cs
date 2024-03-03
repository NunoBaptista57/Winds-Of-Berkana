using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MovingTree : Door
{
    public override void ChangeDoorLook()
    {
        throw new System.NotImplementedException();
    }

    public override void Open()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Move");
        IsOpen = true;
    }
}