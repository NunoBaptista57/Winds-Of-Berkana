using UnityEngine;

public class PlayerBoatBehaviour : MonoBehaviour
{
    protected PlayerBoatEntity player;

    virtual protected void Awake()
    {
        player = GetComponent<PlayerBoatEntity>();
    }
}