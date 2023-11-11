using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrentCollider : MonoBehaviour
{
    [ReadOnlyInspector] public Quaternion direction;
    [ReadOnlyInspector] public float strength;
    [ReadOnlyInspector] public float t;
    
    [HideInInspector] public WindCurrentCollider previous, next;

    WindCurrent parent;

    public float Radius
    {
        set
        {
            transform.localScale = Vector3.one * value * 2;
        }
    }

    void Start()
    {
        parent = transform.parent.GetComponent<WindCurrent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            parent.OnPlayerEnter(this, other.GetComponentInParent<PlayerBoatEntity>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            parent.OnPlayerLeave(this);
    }
}
