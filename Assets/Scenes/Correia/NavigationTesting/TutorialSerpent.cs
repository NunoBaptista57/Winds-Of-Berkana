using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TutorialSerpent : MonoBehaviour
{
    [Header("References (Temp)")]
    public SplineAnimate splineAnimate;
    public Transform playerPos;

    [Header("Settings")]
    public float speed;
    public float maxDistance;
    public float angleThreshold;
    private bool playerInShip = false;

    void begin(){
        playerInShip = true;
    }

    void Update()
    {
        if (playerInShip)
        {
            Vector3 toPlayer = playerPos.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

            if (angleToPlayer > angleThreshold && toPlayer.magnitude <= maxDistance)
            {
                splineAnimate.Play();
            }
            else
            {
                splineAnimate.Pause();
            }
        }
    }
}
