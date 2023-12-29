using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColor : MonoBehaviour
{
    private Key _closestKey;
    private bool _collectedKeys;
    private List<Key> _keys;

    private void Start()
    {
        InvokeRepeating(nameof(GetClosestKey), 0, 3);
        GetClosestKey();
    }

    public void UpdateKeys()
    {
        foreach (Key key in _keys)
        {
            if (!key.Collected)
            {
                GetClosestKey();
                return;
            }
        }
        KeysWereCollected();
    }

    // Get Closest key from the List
    public void GetClosestKey()
    {
        _keys = ServiceLocator.Instance.GetService<KeyManager>().Keys;

        float maxDistance = float.MaxValue;

        foreach (var p in _keys)
        {
            if (p.Collected)
            {
                continue;
            }

            var distance = Vector3.Distance(p.transform.position, transform.position);

            if (distance < maxDistance)
            {
                maxDistance = distance;
                _closestKey = p;
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_collectedKeys)
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        // Get Distance to closest sphere
        if (_closestKey != null)
        {
            var currentDistance = Vector3.Distance(_closestKey.transform.position, transform.position);

            // Change color of the sphere incrementally
            if (currentDistance > 30)
            {
                gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", .99f + ((currentDistance - 16) * 0.0005f)); //color = new Color(255 - distance * 5, 0, 98, 255);*/
            }
            else if (currentDistance > 20)
            {
                gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", .9f + ((currentDistance - 20) * 0.009f)); //color = new Color(255 - distance * 5, 0, 98, 255);*/
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", currentDistance * 0.045f); //color = new Color(255 - distance * 5, 0, 98, 255);*/
            }
        }
    }

    private void KeysWereCollected()
    {
        _collectedKeys = true;
        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1);
    }
}
