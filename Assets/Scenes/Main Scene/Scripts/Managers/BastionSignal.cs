using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Bastion Signal")]
public class BastionSignal : ScriptableObject
{
    public event UnityAction UpdateBastionEvent;

    public void InvokeUpdateBastionEvent()
    {
        UpdateBastionEvent.Invoke();
    }
}