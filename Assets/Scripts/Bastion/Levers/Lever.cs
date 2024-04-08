using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Lever : MonoBehaviour
{
    [HideInInspector] public bool IsActivated = false;
    private bool _playerIsNear = false;
    private LeverManager _leverManager;
    private PlayerActions _playerActions;

    public abstract void ChangeLeverLook();

    public abstract void PlayLeverAnimation();
    
    public void Activate(InputAction.CallbackContext context)
    {
        if (!_playerIsNear || IsActivated)
        {
            return;
        }
        IsActivated = true;
        Debug.Log("Lever activated");
        PlayLeverAnimation();
        _leverManager.ActivateLever(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;
        }
    }

    private void Awake()
    {
        if (transform.parent.TryGetComponent(out LeverManager leverManager))
        {
            _leverManager = leverManager;
        }
        else
        {
            string error = "LeverManager is not parent of " + gameObject.name;
            throw new UnityException(error);
        }
    }


    // TODO this is not efficientz
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