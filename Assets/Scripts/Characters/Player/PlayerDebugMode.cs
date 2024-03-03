using System.Collections;
using UnityEngine;

public class PlayerDebugMode : MonoBehaviour
{
    public bool DebugMode = false;
    public Vector2 Input = Vector2.zero;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform _cameraPosition;
    private bool _goingUp = false;

    public void GoUp()
    {
        _goingUp = true;
        StartCoroutine(GoingUp());
    }

    public void StopGoingUp()
    {
        _goingUp = false;
    }

    IEnumerator GoingUp()
    {
        while (_goingUp)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.up);
            yield return null;
        }
    }

    private void Update()
    {
        if (!DebugMode)
        {
            Input = Vector2.zero;
            return;
        }

        Vector3 velocity = _cameraPosition.forward * Input.y +
                           _cameraPosition.right * Input.x;
        velocity.y = 0;

        transform.Translate(speed * Time.deltaTime * velocity);
    }
}