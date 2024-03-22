using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    static WindManager instance;

    static HashSet<IWindEffector> currentEffectors = new HashSet<IWindEffector>();

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
        foreach (var current in currentEffectors)
        {
            //Debug.Log(current.Force);
            player.AddForce(current.Force, ForceMode.Acceleration);
            var forceDir = current.Force.normalized;
            var torqueAxis = Vector3.Cross(player.transform.forward, forceDir).normalized;
            var directionFactor = Vector3.Angle(player.transform.forward, forceDir) / 180;
            player.AddTorque(current.Force.magnitude * TorqueMultiplier * directionFactor * torqueAxis, ForceMode.Acceleration);
        }
    }

    public static void OnPlayerEnter(IWindEffector entered)
    {
        currentEffectors.Add(entered);
        PlayerBoatEntity.instance.movement.currentSpeed = instance.MaxVelocityInCurrent;
    }

    public static void OnPlayerLeave(IWindEffector left)
    {
        currentEffectors.Remove(left);
        if (currentEffectors.Count == 0)
        {
            PlayerBoatEntity.instance.movement.currentSpeed = PlayerBoatEntity.instance.movement.MaxVelocity;
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
