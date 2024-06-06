using UnityEngine;

public class KeepOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        KeepOnLoad[] instances = FindObjectsOfType<KeepOnLoad>();
        if (instances.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
