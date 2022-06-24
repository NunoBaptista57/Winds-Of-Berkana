using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");

    private GameObject closestSphere;
    private KeyManager _keyManager;
    private bool _keysCollected;

    private float _angle;
    private Color _color = new Color(1, 0, 0);

    void Start()
    {
        _keyManager = KeyManager.Instance;
        _keyManager.KeysCollected += KeysWereCollected;
        InvokeRepeating("GetClosestKey", 0, 3);
    }

    //Removes collected key from the List
    public void RemoveKey(GameObject key)
    {
        puzzle_piece.Remove(key);
    }

    // Get Closest key from the List
    public void GetClosestKey()
    {
        float maxDistance = float.MaxValue;

        foreach (var p in puzzle_piece)
        {
            var distance = Vector3.Distance(p.transform.position, transform.position);

            if (distance < maxDistance)
            {
                maxDistance = distance;
                closestSphere = p;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_keysCollected)
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        // Get Distance to closest sphere
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

    public void KeysWereCollected()
    {
        _keysCollected = true;
        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1);
    }
}
