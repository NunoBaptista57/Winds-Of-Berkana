using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private GameObject _content;
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

    private void Start()
    {
        _content = transform.GetChild(0).gameObject;
    }

    private void Awake()
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