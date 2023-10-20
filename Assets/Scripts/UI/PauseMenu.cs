using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void Start()
    {
        LevelManager.OnGameStateChanged += Pause;
    }

    private void Pause(GameState gameState)
    {
        if (gameState == GameState.Play)
        {
            gameObject.SetActive(false);
        }
        else if (gameState == GameState.Paused)
        {
            gameObject.SetActive(true);
        }
    }
}