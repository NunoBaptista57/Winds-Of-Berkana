using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 10f * Time.deltaTime, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        // collision.collider.gameObject.transform.Translate(collision.impulse);
    }
}