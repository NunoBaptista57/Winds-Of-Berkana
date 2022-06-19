using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{

    MainGameManager manager;
    Transform player;

    Vector3 originalCameraPosition;

    [Serializable]
    public struct SpawnLocation
    {
        public LevelState state;
        public Vector3 position;
    }

    public SpawnLocation[] spawnLocations;

    void Awake()
    {
        manager = MainGameManager.Instance;
        MainGameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalCameraPosition = GameObject.Find("Cameras").GetComponent<Transform>().position;
    }



    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.Respawn)
        {
            var location = Vector3.zero;
            foreach(var spawnL in spawnLocations)
            {
                if(spawnL.state == manager.levelState)
                {
                    location = spawnL.position;
                    break;

                }
            }

            player.position = location;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject.Find("Cameras").GetComponent<Transform>().position = originalCameraPosition;
            MainGameManager.Instance.UpdateGameState(GameState.Play);
        }
    }


}
