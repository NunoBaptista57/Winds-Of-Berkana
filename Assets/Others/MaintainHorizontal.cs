using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainHorizontal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var rot = transform.eulerAngles;
        rot.x = 0;
        transform.eulerAngles = rot;
    }
}
