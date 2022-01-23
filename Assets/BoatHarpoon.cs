using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoatHarpoon : MonoBehaviour
{
    [SerializeField] new CinemachineVirtualCamera camera;
    [SerializeField] CableComponent cable;
    [SerializeField] LayerMask targetHitMask;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] [Min(0)] float RaycastLength = 1000;
    [SerializeField] float ShootTime = 0.5f;
    [SerializeField] float cableLengthOffset;
    [SerializeField] PlayerBoatEntity player;
    [SerializeField] float TorqueLimitMargin = 1;
    [SerializeField, Min(0)] float TorqueStrength = 1;

    Transform end;
    bool grappling;
    float cableLength;

    public string AnimationState;

    ConfigurableJoint joint;

    public bool Shoot()
    {
        Vector3 origin = camera.State.FinalPosition;
        Vector3 direction = camera.State.FinalOrientation * Vector3.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, RaycastLength, raycastMask))
        {
            if (targetHitMask.HasLayer(hit.collider.gameObject.layer))
            {
                StartCoroutine(ShootCoroutine(cable.transform.position, hit.point));
                return true;
            }
        }
        return false;
    }

    IEnumerator ShootCoroutine(Vector3 start, Vector3 target)
    {
        end = new GameObject(name + " Target").transform;
        end.position = start;
        cable.endPoint = end;
        cable.cableLength = Mathf.Max(0, Vector3.Distance(start, target) + cableLengthOffset);
        cable.Activate();
        float t = 0;
        while (t < ShootTime)
        {
            yield return 0;
            t += Time.deltaTime;
            end.position = Vector3.Lerp(start, target, t / ShootTime);
        }
        end.position = target;

        cableLength = Vector3.Distance(transform.position, target);
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = end.position;
        joint.anchor = player.transform.InverseTransformPoint(transform.position);
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        /*
        joint.spring = spring;
        joint.damper = damper;
        joint.massScale = massScale;
        */

        // Distance grapple will try to keep from grapple point
        //joint.maxDistance = cableLength;
        //joint.minDistance = 0;

        joint.linearLimit = new SoftJointLimit
        {
            limit = cableLength,
            bounciness = 0,
            contactDistance = 1f
        };
        joint.enablePreprocessing = false;

        grappling = true;
    }

    public void Release()
    {
        cable.Deactivate();
        grappling = false;
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            //Debug.Log(Vector3.Distance(end.position, transform.position));
        }
        Debug.Log(player.rigidbody.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (grappling)
        {
            var fromTarget = transform.position - end.position;
            if (fromTarget.magnitude >= cableLength - TorqueLimitMargin)
            {
                var projection = Vector3.Project(player.transform.forward, fromTarget.normalized);
                if (projection.Sign() == fromTarget.Sign())
                {
                    var objective = Vector3.ProjectOnPlane(player.transform.forward, fromTarget.normalized).normalized;
                    var torque = Vector3.Cross(player.transform.forward, objective) * TorqueStrength;
                    player.rigidbody.AddTorque(torque, ForceMode.Acceleration);
                }
            }

        }
    }
}
