using UnityEngine;
using System;

public interface IDoor
{
    public void Open();
    public bool CanOpen();
    public bool IsOpen();
}
