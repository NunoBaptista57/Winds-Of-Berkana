using UnityEngine;

public class PlayerBoatEntity : MonoBehaviour
{

    public static PlayerBoatEntity instance { get; private set; } = null;
    public BoatMovement movement { get; private set; }
    new public Rigidbody rigidbody { get; private set; }

    [ReadOnlyInspector] public SailMode sailMode = SailMode.Open;

    [HideInInspector] public PlayerBoatState state = PlayerBoatState.Idle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            movement = GetComponent<BoatMovement>();
            rigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}