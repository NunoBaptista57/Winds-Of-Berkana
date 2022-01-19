using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPieceShader : MonoBehaviour
{

    Material mat;
    float value;

    void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
    }

    /*// Update is called once per frame
    void FixedUpdate()
    {
        value = 
        mat.SetFloat()
    }*/

}
