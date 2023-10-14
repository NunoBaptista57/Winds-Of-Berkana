using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColor : MonoBehaviour
{
    [SerializeField] private List<GameObject> _keys;

    private GameObject closestSphere;
    private bool _keysCollected;

    void Start()
    {
        InvokeRepeating(nameof(GetClosestKey), 0, 3);
        GetClosestKey();
    }

    //Removes collected key from the List
    public void RemoveKey(GameObject key)
    {
        _keys.Remove(key);
    }

    // Get Closest key from the List
    public void GetClosestKey()
    {
        float maxDistance = float.MaxValue;

        foreach (var p in _keys)
        {
            if (p.GetComponent<IKey>().IsCollected())
            {
                continue;
            }

            var distance = Vector3.Distance(p.transform.position, transform.position);

            if (distance < maxDistance)
            {
                maxDistance = distance;
                closestSphere = p;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_keysCollected)
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        // Get Distance to closest sphere
        if (closestSphere != null)
        {
            var currentDistance = Vector3.Distance(closestSphere.transform.position, this.transform.position);

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

    public void KeysWereCollected()
    {
        _keysCollected = true;
        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1);
    }

    private void UpdateKeys()
    {
        foreach (GameObject key in _keys)
        {
            if (key.GetComponent<IKey>().IsCollected())
            {
                GetClosestKey();
                return;
            }
        }
        KeysWereCollected();
    }

    private void OnEnable()
    {
        // _eventSender.CollectedKeyEvent += UpdateKeys;
    }

    private void OnDisable()
    {
        // _eventSender.CollectedKeyEvent -= UpdateKeys;
    }
}
