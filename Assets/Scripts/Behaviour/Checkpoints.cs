using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private Transform playerSpawnPoint;
    private Transform birdSpawnPoint;
    private int highestCheckpointReached = -1;

    public void SetCheckpoint(int checkpointNumber, Transform playerLocation)
    {
        if (checkpointNumber > highestCheckpointReached)
        {
            print("Current Checkpoint = " + checkpointNumber.ToString());
            highestCheckpointReached = checkpointNumber;
            playerSpawnPoint = playerLocation;
        }
    }

    public void SetCheckpoint(int checkpointNumber, Transform playerLocation, Transform birdLocation)
    {
        if (checkpointNumber > highestCheckpointReached)
        {
            print("Current Checkpoint = " + checkpointNumber.ToString());
            highestCheckpointReached = checkpointNumber;
            playerSpawnPoint = playerLocation;
            birdSpawnPoint = birdLocation;
        }
    }

    public int GetHighestCheckpointReached()
    {
        return highestCheckpointReached;
    }

    public Transform GetPlayerSpawnPoint()
    {
        return playerSpawnPoint;
    }

    public Transform GetBirdSpawnPoint()
    {
        return birdSpawnPoint;
    }

    public bool isfirstCheckpoint(){
        return highestCheckpointReached == 0;
    }
}
