using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;
    [SerializeField] private GameObject _parentMenu;
    private PlayerActions _playerActions;

    public void GoBack(InputAction.CallbackContext _)
    {
        if (_parentMenu == null)
        {
            return;
        }
        _parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    protected void UpdateFirstSelected()
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));    
        _firstSelected.GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        UpdateFirstSelected();
        _playerActions = new();
        _playerActions.UI.Cancel.performed += GoBack;
        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.UI.Cancel.performed -= GoBack;
        _playerActions.Disable();
    }

    private void OnDisable()
    {
        _playerActions.UI.Cancel.performed -= GoBack;
        _playerActions.Disable();
    }
}