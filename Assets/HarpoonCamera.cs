using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HarpoonCamera : MonoBehaviour
{
    new CinemachinePOV camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
    }


}
