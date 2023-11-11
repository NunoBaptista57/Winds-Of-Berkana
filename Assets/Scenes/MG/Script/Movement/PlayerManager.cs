// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerManager : MonoBehaviour
// {
//     InputManager inputManager;
//     PlayerLocomotion playerLocomotion;
//     Animator animator;

//     public bool isInteracting;
//     public bool isUsingRootMotion;

//     private void Awake()
//     {
//         animator = GetComponent<Animator>();
//         inputManager = GetComponent<InputManager>();
//         playerLocomotion = GetComponent<PlayerLocomotion>();
//     }

//     void Update()
//     {

//         inputManager.HandleAllInputs();
//     }

//     void FixedUpdate()
//     {
//         playerLocomotion.HandleAllMovement();

//     }

//     void LateUpdate()
//     {
//         isInteracting = animator.GetBool("isInteracting");
//         isUsingRootMotion = animator.GetBool("isUsingRootMotion");
//         playerLocomotion.isJumping = animator.GetBool("isJumping");
//         animator.SetBool("isGrounded", playerLocomotion.isGrounded);
//     }
// }
