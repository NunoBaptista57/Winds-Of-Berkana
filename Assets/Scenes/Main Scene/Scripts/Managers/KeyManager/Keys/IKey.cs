using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKey
{
    public void SetKeyManager(KeyManager keyManager)
    public void Collect();
    public bool IsCollected();
}
