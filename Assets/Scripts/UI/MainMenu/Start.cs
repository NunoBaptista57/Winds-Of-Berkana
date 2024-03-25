using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Start : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    private PlayerActions _playerActions;

    public void StartGame(InputAction.CallbackContext context)
    {
        _mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _playerActions = new();
        _playerActions.UI.AnyButton.started += StartGame;
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.UI.AnyButton.started -= StartGame;
        _playerActions.Disable();
    }
}