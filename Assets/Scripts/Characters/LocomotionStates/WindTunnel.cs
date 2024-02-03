using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTunnel : MonoBehaviour, ILocomotionState
{
    public GameObject pointB;
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 10f;


    private CharacterLocomotion _characterLocomotion;

    public void StartJump()
    {
    }

    public void StopJump()
    {

    }

    public void Move()
    {
        float maxSpeed = _maxSpeed;

        _characterLocomotion.Rotate(_rotationSpeed);

        Vector3 newVelocity = pointB.transform.position - _characterLocomotion.Body.position;
        Debug.Log(newVelocity);
        if (newVelocity.x < 1 && newVelocity.y < 1 && newVelocity.z < 1)
        {
            Debug.Log("Entered fall state");
            Fall();
        }
        newVelocity.Normalize();
        newVelocity = newVelocity * maxSpeed;

        _characterLocomotion.NewVelocity = newVelocity;

    }

    public void Run()
    {

    }

    public void Fall()
    {
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground()
    {
    }

    public void Tunnel()
    {

    }

    public void Walk(bool walk)
    {

    }

    public void StartState()
    {
    }

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}
