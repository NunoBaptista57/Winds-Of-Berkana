using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Events;

public class PlayerBoatHarpoonController : PlayerBoatBehaviour
{
    [SerializeField] Animator CameraAnimator = null;
    [SerializeField] float AimSpeed = 1;
    [SerializeField] BoatHarpoon RightHarpoon = null;
    [SerializeField] BoatHarpoon LeftHarpoon = null;
    BoatHarpoon currentHarpoon;

    [SerializeField] UnityEvent OnHarpoonShoot;
    [SerializeField] UnityEvent OnHarpoonCanceled;

    bool toShoot;

    void LateUpdate()
    {
        if (toShoot)
        {
            CameraAnimator.Play("Idle");
            player.state = PlayerBoatState.Transition;
            currentHarpoon.Shoot();
            currentHarpoon = null;
            OnHarpoonShoot.Invoke();
        }
        toShoot = false;
    }

    void OnRightHarpoon(InputValue value)
    {
        OnHarpoon(value, RightHarpoon);
    }

    void OnLeftHarpoon(InputValue value)
    {
        OnHarpoon(value, LeftHarpoon);
    }

    void OnHarpoon(InputValue value, BoatHarpoon harpoon)
    {
        if (player.state == PlayerBoatState.Idle && !currentHarpoon)
        {
            CameraAnimator.Play(harpoon.AnimationState);
            player.state = PlayerBoatState.Transition;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            currentHarpoon = harpoon;
            if (currentHarpoon.Grappling)
            {
                currentHarpoon.Release();
            }
        }
    }

    void OnShootHarpoon(InputValue value)
    {
        if (player.state == PlayerBoatState.Harpoon && currentHarpoon && !toShoot)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            toShoot = true;
        }
    }

    void OnCancelHarpoon(InputValue value)
    {
        if (player.state == PlayerBoatState.Harpoon && currentHarpoon && !toShoot)
        {
            CameraAnimator.Play("Idle");
            player.state = PlayerBoatState.Transition;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            currentHarpoon = null;
            OnHarpoonCanceled.Invoke();
        }
    }

    public void TransitionToIdle()
    {
        player.state = PlayerBoatState.Idle;
        Debug.Log(player.state);
    }

    public void TransitionToHarpoon()
    {
        player.state = PlayerBoatState.Harpoon;
        Debug.Log(player.state);
    }

    void OnRelease(InputValue value)
    {
        LeftHarpoon.Release();
        RightHarpoon.Release();
    }
}
