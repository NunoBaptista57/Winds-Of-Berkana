using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bastion1EndGameHandler : MonoBehaviour
{

    public Bastion1LevelManager levelManager;
    public GameObject textObject;
    private Animator animator;

    public bool elevatorDown = false;

    public GameObject[] removeCollisions;

    private GameObject player;

    private void Start()
    {
        levelManager.OnLevelStateChanged += EndGameHandler;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += SendElevator;
        animator = this.transform.GetComponent<Animator>();
    }


    public void EndGameHandler(LevelState levelState)
    {
       if(levelState == LevelState.BastionState_Ending)
        {
          
            CallElevator(); 
        }
    }


    void CallElevator()
    {
        animator.SetTrigger("Down");
    }

    public void ElevatorIsDown()
    {
        elevatorDown = true;
    }


    void SendElevator()
    {
        if (textObject.activeSelf && elevatorDown) {
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
