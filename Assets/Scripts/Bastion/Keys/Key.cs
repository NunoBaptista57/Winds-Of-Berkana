using UnityEngine;

public abstract class Key : MonoBehaviour
{
    [HideInInspector] public bool Collected = false;
    private KeyManager _keyManager;

    public abstract void PlayCollectKeyAnimation();

    public void Collect()
    {
        Collected = true;
        _keyManager.CollectKey(this);
        PlayCollectKeyAnimation();
    }

    private void Awake()
    {
        if (transform.parent.TryGetComponent(out KeyManager keyManager))
        {
            _keyManager = keyManager;
        }
        else
        {
            string error = "KeyManager is not parent of " + gameObject.name;
            throw new UnityException(error);
        }
    }
}
