using System;
using UnityEngine;

public interface ILever
{
    public bool ToActivate();
    public void SetActivated(bool activated);
    public bool IsActivated();
    public int GetID();
}