using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatisGrappeable;
    public Transform gunTip, camera, player;
    public float maxDistance = 100f;
    private SpringJoint joint;
    private bool fired = false;



    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        camera = Camera.main.GetComponent<Transform>();
    }


    void LateUpdate()
    {
        if(fired)
            DrawRope();
    }

    public void StartGrapple()
    {
        RaycastHit hit;
       // Debug.Log("Starting Grapple");
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatisGrappeable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // Distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Change these values to fit whatever you like
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            fired = true;

        }
    }


    void DrawRope()
    {

            if (!joint) return;
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        fired = false;
    }





}
