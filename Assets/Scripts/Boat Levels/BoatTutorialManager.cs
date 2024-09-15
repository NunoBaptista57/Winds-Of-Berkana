using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTutorialManager : MonoBehaviour
{
    [SerializeField]
    private Beacon[] beacons;

    private int _currBeacon = 0;
    void Start()
    {
        
    }

    public void Next()
    {
        _currBeacon++;

        if( _currBeacon < beacons.Length)
        {
            beacons[_currBeacon].Appear();
        }
    }
}
