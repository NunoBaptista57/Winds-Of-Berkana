using UnityEngine;
using System;



public class Key : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;

    private void Collect()
    {
        _eventManager.InvokeCollectedKeyEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }
}
// {
//     [SerializeField] private float degreesPerSecond = 15.0f;
//     [SerializeField] private float amplitude = 0.5f;
//     [SerializeField] private float frequency = 1f;
//     [SerializeField] private int _pieceNumber;

//     public event Action<int> Collect;

//     // Position Storage Variables
//     Vector3 posOffset = new();
//     Vector3 tempPos = new();

//     // Use this for initialization
//     void Start()
//     {
//         // Store the starting position & rotation of the object
//         posOffset = transform.position;
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         //Spin object around Y-Axis
//         transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

//         // Float up/down with a Sin()
//         tempPos = posOffset;
//         tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

//         transform.position = tempPos;
//     }

// }
