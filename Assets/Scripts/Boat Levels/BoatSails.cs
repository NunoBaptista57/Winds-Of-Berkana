using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSails : MonoBehaviour
{
    private float sails_size;
    // Start is called before the first frame update
    void Start()
    {
        sails_size = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.P))
        {
            RaiseSails();
        }
        if (Input.GetKey(KeyCode.O))
        {
            LowerSails();
        }
    }

    public void RaiseSails()
    {
        sails_size = sails_size+0.1f;
        sails_size = Mathf.Clamp(sails_size, 0.2f, 5);
        this.transform.localScale = new Vector3(sails_size, 3, .1f);
    }

    public void LowerSails()
    {
        sails_size = sails_size - 0.1f;
        sails_size = Mathf.Clamp(sails_size, 0.2f, 5);
        this.transform.localScale = new Vector3(sails_size, 3, .1f);
    }
}
