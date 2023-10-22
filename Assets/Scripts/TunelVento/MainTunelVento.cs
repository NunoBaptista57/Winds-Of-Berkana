using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Cinemachine;
using Vector3 = UnityEngine.Vector3;

public class MainTunelVento : MonoBehaviour
{
    public GameObject _player;

    private bool overworldCamera = true;

    public GameObject pointB;
    public float rate;
    
    [SerializeField]
    private CinemachineFreeLook playerCam;
    [SerializeField]
    private CinemachineFreeLook vcam2;
    private Vector3 initialPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPlayerPosition = _player.transform.position;
        Vector3 position = pointB.transform.position;
        //Vector3 position_corr = (position.x, position.y * 0.001, position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider target)
    {
        Debug.Log("Poop");
        if (target.gameObject.CompareTag("Player"))
        {
            SwitchPriority();
            //target.gameObject.transform.position = Vector3.MoveTowards(target.gameObject.transform.position, pointB.transform.position, rate);
            StartCoroutine(MovePlayerToPointB(target.gameObject));

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwitchPriority();
        }
    }

    void SwitchPriority()
    {
        if (overworldCamera)
        {
            vcam2.Priority = playerCam.Priority;
            playerCam.Priority = 0;
        }
        else
        {
            playerCam.Priority = vcam2.Priority;
            vcam2.Priority = 0;   
        }

        overworldCamera = !overworldCamera;
    }
    
    IEnumerator MovePlayerToPointB(GameObject player)
    {
        float journeyLength = Vector3.Distance(player.transform.position, pointB.transform.position);
        float journeyTime = journeyLength / rate;
        float startTime = Time.time;
        float distanceCovered = 0;

        while (distanceCovered < journeyLength)
        {
            float distance = (Time.time - startTime) * rate;
            float fractionOfJourney = distance / journeyLength;
            player.transform.position = Vector3.Lerp(initialPlayerPosition, pointB.transform.position, fractionOfJourney);
            distanceCovered = Vector3.Distance(player.transform.position, initialPlayerPosition);
            yield return null;
        }
    }
}
