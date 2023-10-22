using System;
using UnityEngine;

public interface ILever
{

    // TODO: this is too complex
    public bool ToActivate();
    public void SetActivated(bool activated);
    public bool IsActivated();
    public int GetID();
}