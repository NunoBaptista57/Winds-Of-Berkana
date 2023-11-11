using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Death);
        }
    }
}