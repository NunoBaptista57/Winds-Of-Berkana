using UnityEngine;

public class PlayerBoatBehaviour2 : MonoBehaviour
{
    protected PlayerBoatEntity player;

    virtual protected void Awake()
    {
        player = GetComponent<PlayerBoatEntity>();
    }
}