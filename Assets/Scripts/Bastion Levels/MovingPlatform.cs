using System;
using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _distance = 10f;
    private Vector3 _direction;
    private Vector3 _initialPosition;
    private Rigidbody _rigidbody;

    private void Update()
    {
        _rigidbody.MovePosition(transform.position + _speed * Time.deltaTime * _direction);
        if (Vector3.Distance(transform.position, _initialPosition) >= _distance)
        {
            _direction *= -1;
            _initialPosition = transform.position;
        }
    }

    private void Start()
    {
        _direction = transform.forward;
        _initialPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }
}