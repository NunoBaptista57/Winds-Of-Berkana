using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    static WindManager instance;

    static HashSet<WindCurrent> currentCurrents = new HashSet<WindCurrent>();

    public float TorqueMultiplier = 1;
    public float MaxVelocityInCurrent = 20;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody player = PlayerBoatEntity.instance.rigidbody;
        foreach (var current in currentCurrents)
        {
            //Debug.Log(current.Force);
            player.AddForce(current.Force, ForceMode.Acceleration);
            var forceDir = current.Force.normalized;
            var torqueAxis = Vector3.Cross(player.transform.forward, forceDir).normalized;
            var directionFactor = Vector3.Angle(player.transform.forward, forceDir) / 180;
            Debug.Log(directionFactor);
            player.AddTorque(current.Force.magnitude * TorqueMultiplier * directionFactor * torqueAxis, ForceMode.Acceleration);
        }
    }

    public static void OnPlayerEnter(WindCurrent entered)
    {
        currentCurrents.Add(entered);
        PlayerBoatEntity.instance.movement.CurrentMaxVelocity = instance.MaxVelocityInCurrent;
    }

    public static void OnPlayerLeave(WindCurrent left)
    {
        currentCurrents.Remove(left);
        if (currentCurrents.Count == 0)
        {
            PlayerBoatEntity.instance.movement.CurrentMaxVelocity = PlayerBoatEntity.instance.movement.MaxVelocity;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
