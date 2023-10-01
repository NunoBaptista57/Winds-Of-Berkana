using System.Collections.Generic;
using UnityEngine;

public class MovingTree : MonoBehaviour, IDoor
{
    [SerializeField] private List<GameObject> _keys;

    private bool _isOpen = false;

    public void Open()
    {
        _isOpen = true;
        gameObject.GetComponent<Animator>().SetTrigger("Move");
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    public bool CanOpen()
    {
        foreach (GameObject key in _keys)
        {
            if (!key.GetComponent<IKey>().IsCollected())
            {
                return false;
            }
        }
        return true;
    }
}