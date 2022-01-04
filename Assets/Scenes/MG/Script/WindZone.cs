using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
  
    public enum direction { up, down, left , right};

    public direction windDirection;
    public float strength;

    private Vector3 auxDirection;
    
    void Awake()
    {

        switch (windDirection)
        {
            case direction.up:
                auxDirection = new Vector3(0.0f, 1.0f, 0.0f);
                break;
            case direction.down:
                auxDirection = new Vector3(0.0f, -1.0f, 0.0f);
                break;
            case direction.right:
                auxDirection = new Vector3(1.0f, 0.0f, 0.0f);
                break;
            case direction.left:
                auxDirection = new Vector3(-1.0f, 1.0f, 0.0f);
                break;

        }
    
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(auxDirection * strength, ForceMode.Impulse);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(auxDirection * strength, ForceMode.Impulse);
        }
    }


}
