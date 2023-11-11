using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerLocomotion _playerLocomotion;

    public void SetCanMove(bool canMove)
    {
        _playerLocomotion.canMove = canMove;
    }
}