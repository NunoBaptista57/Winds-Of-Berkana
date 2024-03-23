using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    private LevelManager _levelManager;
    private PlayerActions _playerActions;

    public void Pause(InputAction.CallbackContext context)
    {
        if (_content.activeSelf)
        {
            Resume();
            return;
        }
        _content.SetActive(true);
        _levelManager.Pause(true);
    }
    public void Resume()
    {
        Debug.Log("Click");
        _content.SetActive(false);
        _levelManager.Pause(false);
    }

    public void Load()
    {
        _levelManager.Load();
    }

    public void Save()
    {
        _levelManager.Save();
    }

    public void Restart()
    {
        _levelManager.Restart();
    }

    public void Quit()
    {
        _levelManager.Quit();
    }

    private void Start()
    {
        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
    }

    private void OnEnable()
    {
        _playerActions = new();

        _playerActions.UI.Pause.performed += Pause;

        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.UI.Pause.performed -= Pause;
        _playerActions.Disable();
    }
}