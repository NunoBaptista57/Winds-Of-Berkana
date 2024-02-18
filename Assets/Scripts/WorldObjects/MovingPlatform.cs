using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public Transform Transform;

    private void Update()
    {
        Rigidbody.Move(Transform.position * Time.deltaTime, transform.rotation);
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}