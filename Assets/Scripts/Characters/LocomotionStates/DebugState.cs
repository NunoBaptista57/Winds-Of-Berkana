using UnityEngine;

public class DebugState : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private bool _goingUp = false;
    private Transform _player;

    public void Move()
    {
        if (_characterLocomotion.Input.magnitude < 0.1f)
        {
            return;
        }

        float baseRotation = _characterLocomotion.BasePosition.rotation.eulerAngles.y;
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, baseRotation, transform.parent.rotation.z);
        float targetAngle = Vector2.SignedAngle(_characterLocomotion.Input, Vector2.up);
        _characterLocomotion.Body.transform.localRotation = Quaternion.Euler(_characterLocomotion.Body.transform.rotation.x, targetAngle, _characterLocomotion.Body.transform.rotation.z);

        Vector3 newVelocity = speed * Time.deltaTime * _characterLocomotion.Body.forward;
        Debug.Log(newVelocity);
        if (_goingUp)
        {
            newVelocity.y = speed * Time.deltaTime;
        }

        _player.Translate(newVelocity, Space.World);
    }

    public void StartJump()
    {
        _goingUp = true;
    }

    public void StopJump()
    {
        _goingUp = false;
    }

    private void Awake()
    {
        _player = transform.parent.transform;
        Debug.Log(_player.gameObject.name);
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}