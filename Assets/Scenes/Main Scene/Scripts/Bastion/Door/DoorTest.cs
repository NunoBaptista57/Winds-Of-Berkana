using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour, IDoor
{
    [SerializeField] private List<GameObject> _keys;
    public void Open()
    {
        Debug.Log("Open");
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

    public bool IsOpen()
    {
        return false;
    }
}