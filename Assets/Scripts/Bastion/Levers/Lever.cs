using AmplifyShaderEditor;
using UnityEngine;

public abstract class Lever : MonoBehaviour
{
    [HideInInspector] public bool IsActivated = false;
    private bool _playerIsNear = false;
    private LeverManager _leverManager;

    // TODO: interaction
    public abstract void ChangeLeverLook();

    public abstract void PlayLeverAnimation();

    public void Activate()
    {
        if (!_playerIsNear || IsActivated)
        {
            return;
        }
        IsActivated = true;
        PlayLeverAnimation();
        _leverManager.ActivateLever(gameObject.name);
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
        if (TryGetComponent(out LeverManager leverManager))
        {
            _leverManager = leverManager;
        }
        else
        {
            string error = "LeverManager is not parent of " + gameObject.name;
            throw new UnityException(error);
        }
    }
}