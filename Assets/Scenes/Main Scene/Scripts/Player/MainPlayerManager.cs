using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerManager : MonoBehaviour
{
    MainPlayerInputHandler inputManager;
    MainPlayerLocomotion playerLocomotion;
    Animator animator;

    public bool isInteracting;
    public bool isUsingRootMotion;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<MainPlayerInputHandler>();
        playerLocomotion = GetComponent<MainPlayerLocomotion>();
    }

    void Update()
    {

        inputManager.HandleAllInputs();
    }

    void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();

    }

    void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
