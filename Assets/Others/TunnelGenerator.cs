using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [SerializeField] GameObject toClone;
    [SerializeField] float Radius;
    [SerializeField] [Min(1)] uint Clones;

    [ContextMenu("Generate")]
    void Start()
    {
        for (int i = 0; i < Clones; i++)
        {
            Instantiate(toClone,
                        transform.position + Radius * new Vector3(
                            Mathf.Cos(2 * Mathf.PI / Clones * i),
                            Mathf.Sin(2 * Mathf.PI / Clones * i),
                            0),
                        Quaternion.identity,
                        transform);
        }
    }

    [ContextMenu("Clean")]
    void Clean()
    {
        foreach (Transform c in GetComponentsInChildren<Transform>())
        {
            if (c != transform)
                DestroyImmediate(c.gameObject);
        }
    }
}
