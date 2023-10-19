using UnityEngine;

public interface IKey
{
    public void Collect();
    public bool IsCollected();
    public Transform GetPosition();
    public void SetKeyManager(KeyManager keyManager);
}
