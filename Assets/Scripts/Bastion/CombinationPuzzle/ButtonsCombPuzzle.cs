using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;

public class ButtonsCombPuzzle : MonoBehaviour
{

    private PlayerActions _playerActions;

    [SerializeField]
    private bool isNear;
    [Header("Canvas Objects")]
    //public TextMeshProUGUI infoText;
    public bool activePart = false;
    public event Action<ButtonsCombPuzzle> OnActivated;


    private CombinatioPuzzle _combPuzzle;

    public void Activate(InputAction.CallbackContext context)
    {
        if (!isNear)
        {
            return;
        }
        if (OnActivated != null)
        {
            OnActivated(this); // Pass the current instance to the event
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            //infoText.gameObject.SetActive(true);
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
     {
        if (other.gameObject.CompareTag("Player"))
        {
            //infoText.gameObject.SetActive(false);
            isNear = false;
        }
     }

    private void OnEnable()
    {
        _playerActions = new();
        _playerActions.Character.Interact.started += Activate;
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Character.Interact.started -= Activate;
        _playerActions.Disable();
    }
}
