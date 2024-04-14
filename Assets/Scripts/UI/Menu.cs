using System.Drawing.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;
    [SerializeField] private GameObject _parentMenu;
    private PlayerActions _playerActions;
    private bool _mouseMode = false;

    public void GoBack(InputAction.CallbackContext _)
    {
        if (_parentMenu == null)
        {
            return;
        }
        _parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnNavigate(InputAction.CallbackContext _)
    {
        if (_mouseMode)
        {
            UpdateFirstSelected();
        }
    }

    private void OnPoint(InputAction.CallbackContext _)
    {
        Cursor.visible = true;
        _mouseMode = true;
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);
    }

    protected void UpdateFirstSelected()
    {
        _mouseMode = false;
        Cursor.visible = false;
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));    
        _firstSelected.GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        UpdateFirstSelected();
        _playerActions = new();
        _playerActions.UI.Navigate.performed += OnNavigate;
        _playerActions.UI.Point.performed += OnPoint;
        _playerActions.UI.Cancel.performed += GoBack;
        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.UI.Cancel.performed -= GoBack;
        _playerActions.UI.Navigate.performed -= OnNavigate;
        _playerActions.UI.Point.performed -= OnPoint;       
        _playerActions.Disable();
    }

    private void OnDisable()
    {
        _playerActions.UI.Cancel.performed -= GoBack;
        _playerActions.UI.Navigate.performed -= OnNavigate;
        _playerActions.UI.Point.performed -= OnPoint;
        _playerActions.Disable();
    }
}