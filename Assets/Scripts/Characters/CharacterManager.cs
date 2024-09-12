using Cinemachine.Utility;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private bool _canMove = true;
    protected CharacterLocomotion CharacterLocomotion;
    protected CharacterAnimation CharacterAnimation;
    protected CharacterController CharacterController;

    public AudioManager audioManager;


    public void Start()
    {
        /* AudioManager audioManager = FindObjectOfType<AudioManager>();

         if (audioManager == null)
         {
             Debug.LogWarning("AudioManager not found in the scene.");
         }
         else
         {
             Debug.Log("AudioManager found.");
         }*/
    }
    public void SetCanMove(bool canMove)
    {
        CharacterController.enabled = canMove;
        CharacterLocomotion.gameObject.SetActive(canMove);
        _canMove = canMove;
    }

    virtual public void Spawn(Transform spawnTransform)
    {
        Debug.Log("Spawning...");
        transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
    }

    public void Move(Vector2 input)
    {
        CharacterLocomotion.Input = input;
    }

    public void Run()
    {
        if (_canMove)
        {
            CharacterLocomotion.Run();
        }
    }

    public void Walk(bool walk)
    {
        if (_canMove)
        {
            CharacterLocomotion.Walk(walk);
        }
    }

    // The bool states wether the jump is starting or ending
    public void Jump(bool startedJump)
    {
        if (!_canMove)
        {
            return;
        }

        if (startedJump)
        {
            CharacterLocomotion.StartJump();
        }
        else
        {
            CharacterLocomotion.StopJump();
        }
    }

    public void ChangeAnimation(CharacterAnimation.AnimationState animationState)
    {
        CharacterAnimation.ChangeAnimation(animationState);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterLocomotion.OnCollision(hit);
    }

    private void Update()
    {
        CharacterAnimation.Animator.SetFloat("HorizontalSpeed", CharacterLocomotion.InputVelocity.ProjectOntoPlane(Vector3.up).magnitude);
    }

    private void Awake()
    {
        CharacterAnimation = GetComponentInChildren<CharacterAnimation>();
        CharacterLocomotion = GetComponentInChildren<CharacterLocomotion>();
        CharacterController = GetComponentInChildren<CharacterController>();
    }
}