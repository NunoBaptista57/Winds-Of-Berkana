using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFreeLookExtension : CinemachineExtension
{
    CinemachineInputProvider inputProvider;

    [SerializeField] float damping = 0.1f;

    [SerializeField] float inputMultiplier = 1f;

    [SerializeField] float MaxVelocity = 1f;

    Vector2 currentAngularVelocity;

    Vector2 currentRotation;

    [SerializeField] float RotateAroundOffset;
    [SerializeField] Transform target;
    [SerializeField] float TimeToRecenter = 2;
    [SerializeField] float recenterStrength = 1;
    [SerializeField] int recenterSteps = 4;

    float timeSinceInput = 0;

    private void Start()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        Debug.Log(inputProvider);
    }

    Vector2 GetInput()
    {
        if (inputProvider)
        {
            return new Vector2(inputProvider.GetAxisValue(0), inputProvider.GetAxisValue(1));
        }
        return new Vector2();
    }

    float LerpAngleRecursive(float a, float b, float t, int numRecursions)
    {
        if (numRecursions < 1)
        {
            return a;
        }
        return Mathf.LerpAngle(LerpAngleRecursive(a, b, t, numRecursions - 1), b, t);
    }

    Vector2 LerpAngleVectorRecursive(Vector2 a, Vector2 b, float t, int numRecursions)
    {
        return new Vector2(LerpAngleRecursive(a.x, b.x, t, numRecursions), LerpAngleRecursive(a.y, b.y, t, numRecursions));
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (target)
        {

            timeSinceInput += deltaTime;
            var input = GetInput();
            if (input != Vector2.zero)
            {
                timeSinceInput = 0;
            }
            currentAngularVelocity = Vector2.Lerp(currentAngularVelocity, Vector2.zero, damping * deltaTime);
            currentAngularVelocity += input * inputMultiplier * deltaTime;
            currentAngularVelocity = Vector2.ClampMagnitude(currentAngularVelocity, MaxVelocity);
            currentRotation += currentAngularVelocity * deltaTime;

            if (timeSinceInput >= TimeToRecenter)
            {
                currentRotation = LerpAngleVectorRecursive(currentRotation, Vector2.zero, recenterStrength * deltaTime, recenterSteps);
            }

            var forward = target.forward;
            forward.y = 0;
            forward.Normalize();

            var rotateCenter = target.position + forward * RotateAroundOffset;

            if (stage == CinemachineCore.Stage.Finalize)
            {
                Quaternion offsetRotation = Quaternion.Euler(currentRotation.x * Vector3.up) * Quaternion.AngleAxis(-currentRotation.y, state.RawOrientation * Vector3.right);
                state.RawOrientation = offsetRotation * state.RawOrientation;
                Vector3 posOffset = state.RawPosition - rotateCenter;
                posOffset = offsetRotation * posOffset;
                state.RawPosition = rotateCenter + posOffset;
            }
        }
    }
}
