using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private LevelManager _levelManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _levelManager.Death();
        }
    }

    private void Start()
    {
        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
    }
}