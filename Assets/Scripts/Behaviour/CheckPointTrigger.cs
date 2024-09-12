using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    public int checkpointNumber;
    public Transform playerSpawnPoint;
    public Transform birdSpawnPoint;
    public Checkpoints checkpointsScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (birdSpawnPoint != null)
            {
                checkpointsScript.SetCheckpoint(checkpointNumber, playerSpawnPoint, birdSpawnPoint);
            }
            else 
            {
                checkpointsScript.SetCheckpoint(checkpointNumber, playerSpawnPoint);
            }
        }
    }
}
