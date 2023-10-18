using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Elevator : MonoBehaviour, IDoor
{
    [SerializeField] private GameObject text;
    private bool elevatorDown = false;
    private Animator animator;
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
        ServiceLocator.instance.GetService<Bastion1Manager>().OnLevelStateChanged += CallElevator;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += SendElevator;
        animator = transform.GetComponent<Animator>();
    }

    private void CallElevator(LevelState levelState)
    {
        if (levelState == LevelState.BastionState_Puzzle3)
        {
            Open();
        }
    }

    public void ElevatorIsDown()
    {
        elevatorDown = true;
    }

    private void SendElevator()
    {
        if (elevatorDown)
        {
            text.SetActive(false);
            animator.SetTrigger("Up");
            elevatorDown = false;

            //TODO: collisions
            // foreach (var v in removeCollisions)
            // {
            //     var col = v.GetComponent<MeshCollider>();
            //     col.convex = true;
            //     col.isTrigger = true;
            // }
        }
    }

    private void OnTriggerEnter()
    {
        text.SetActive(true);
    }
}