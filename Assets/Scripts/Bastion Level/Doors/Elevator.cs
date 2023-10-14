using System;
using UnityEngine;

public class Elevator : MonoBehaviour, IDoor
{
    private Animator animator;

    public bool elevatorDown = false;

    public GameObject[] removeCollisions;

    private GameObject player;
    private bool _isOpen = false;

    public void Open()
    {
        animator.SetTrigger("Down");
        _isOpen = true;
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    private void Start()
    {
        ServiceLocator.instance.GetService<Bastion1LevelManager>().OnLevelStateChanged += CallElevator;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += SendElevator;
        animator = transform.GetComponent<Animator>();
    }

    private void CallElevator(LevelState levelState)
    {
        if (levelState == LevelState.BastionState_Puzzle3)
        {
            Debug.Log("Elevator");
            Open();
        }
    }

    public void ElevatorIsDown()
    {
        elevatorDown = true;
    }

    void SendElevator()
    {
        if (elevatorDown)
        {
            animator.SetTrigger("Up");
            elevatorDown = false;
            foreach (var v in removeCollisions)
            {
                var col = v.GetComponent<MeshCollider>();
                col.convex = true;
                col.isTrigger = true;
            }
        }
    }
}