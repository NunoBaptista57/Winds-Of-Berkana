using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectInitializer : MonoBehaviour
{
    new Renderer renderer;
    static System.Random random = new System.Random();

    private void OnEnable()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Seed", Random.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
