using UnityEngine;

public class Key : MonoBehaviour
{
    [HideInInspector] public KeyManager KeyManager;
    [HideInInspector] public bool Collected = false;

    public void Collect()
    {
        Collected = true;
        gameObject.SetActive(false);
    }
}
