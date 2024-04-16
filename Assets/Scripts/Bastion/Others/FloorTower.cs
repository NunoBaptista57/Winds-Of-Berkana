using UnityEngine;

public class FloorTower : MonoBehaviour
{
    
    public void Open()
    {
        gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(true);
    }
}