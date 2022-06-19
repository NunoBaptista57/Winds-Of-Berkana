using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{

    MainGameManager manager;
    Transform player;

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
    }



    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.Death)
        {
            var location = Vector3.zero;
            Debug.Log("Respawn");
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
        }
    }


}
