using System;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject textObject;
    private Animator animator;

    public bool elevatorDown = false;

    public GameObject[] removeCollisions;

    private GameObject player;

    private void Start()
    {
        ServiceLocator.instance.GetService<Bastion1LevelManager>().OnLevelStateChanged += CallElevator;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += SendElevator;
        animator = transform.GetComponent<Animator>();
    }

    private void CallElevator(LevelState levelState)
    {
        if (levelState == LevelState.BastionState_Ending)
        {
            animator.SetTrigger("Down");
        }
    }

    public void ElevatorIsDown()
    {
        elevatorDown = true;
    }


    void SendElevator()
    {
        if (textObject.activeSelf && elevatorDown)
        {
            animator.SetTrigger("Up");
            elevatorDown = false;
            textObject.SetActive(false);
            foreach (var v in removeCollisions)
            {
                var col = v.GetComponent<MeshCollider>();
                col.convex = true;
                col.isTrigger = true;

            }
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !textObject.activeSelf && elevatorDown)
            textObject.SetActive(true);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && textObject.activeSelf && elevatorDown)
            textObject.SetActive(false);
    }
}