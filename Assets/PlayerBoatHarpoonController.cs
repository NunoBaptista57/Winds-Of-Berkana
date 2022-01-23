using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerBoatHarpoonController : PlayerBoatBehaviour
{
    [SerializeField] Animator CameraAnimator = null;
    [SerializeField] float AimSpeed = 1;
    [SerializeField] BoatHarpoon RightHarpoon = null;
    [SerializeField] BoatHarpoon LeftHarpoon = null;
    BoatHarpoon currentHarpoon;

    bool toShoot;

    void LateUpdate()
    {
        if (toShoot)
        {
            CameraAnimator.Play("Idle");
            player.state = PlayerBoatState.Idle;
            currentHarpoon.Shoot();
            currentHarpoon = null;
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
        if (player.state == PlayerBoatState.Idle)
        {
            CameraAnimator.Play(harpoon.AnimationState);
            player.state = PlayerBoatState.Harpoon;
            currentHarpoon = harpoon;
        }
        else if (player.state == PlayerBoatState.Harpoon && currentHarpoon == harpoon && !toShoot)
        {
            toShoot = true;
        }
    }
}
