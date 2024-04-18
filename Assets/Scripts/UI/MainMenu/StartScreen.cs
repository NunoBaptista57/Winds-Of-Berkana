using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private float _fade = 1.5f;
    private PlayerActions _playerActions;
    //private AudioManager _audioManager;


    private void Start(){

        if(AudioManager.Instance == null){
            Debug.LogError("Audio Manager Null at start");
        }

        AudioManager.Instance.PlayMusic("MainMenuTheme");
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {

        yield return new WaitForSeconds(_fade);
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