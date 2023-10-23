using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AirObstacle : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float speed = 1f;
    private float _timeEnabled;

    public event Action<int> Collect;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    Vector3 direction = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        direction = this.transform.GetChild(1).position - this.transform.GetChild(0).position;
    }

    private void OnEnable()
    {
        _timeEnabled = Time.fixedTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        tempPos.x += (Time.fixedTime - _timeEnabled) * speed * direction.x;
        tempPos.z += (Time.fixedTime - _timeEnabled) * speed * direction.z;

        transform.position = tempPos;
    }
}
