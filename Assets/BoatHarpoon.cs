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
    [SerializeField] float ShootSpeed = 10f;
    [SerializeField] float cableLengthOffset;
    [SerializeField] PlayerBoatEntity player;
    [SerializeField] float TorqueLimitMargin = 1;
    [SerializeField, Min(0)] float TorqueStrength = 1;

    Transform end;
    public bool Grappling { get; private set; }
    bool attached;
    float cableLength;

    public string AnimationState;

    ConfigurableJoint joint;
    Coroutine currentCoroutine;

    public bool Shoot()
    {
        Vector3 origin = camera.State.FinalPosition;
        Vector3 direction = camera.State.FinalOrientation * Vector3.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, RaycastLength, raycastMask))
        {
            if (targetHitMask.HasLayer(hit.collider.gameObject.layer))
            {
                Grappling = true;
                currentCoroutine = StartCoroutine(ShootCoroutine(cable.transform.position, hit.point));
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
        float distance = Vector3.Distance(start, target);
        cable.cableLength = Mathf.Max(0, distance + cableLengthOffset);
        cable.Activate();
        float k = 0;
        while (k < distance)
        {
            yield return 0;
            k += ShootSpeed * Time.deltaTime;
            end.position = Vector3.MoveTowards(start, target, k);
        }
        end.position = target;
        attached = true;

        cableLength = Vector3.Distance(transform.position, target);
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = end.position;
        joint.anchor = player.transform.InverseTransformPoint(transform.position);
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        joint.linearLimit = new SoftJointLimit
        {
            limit = cableLength,
            bounciness = 0,
            contactDistance = 1f
        };
        currentCoroutine = null;
    }

    public void Release()
    {
        cable.Deactivate();
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        Grappling = false;
    }

    private void LateUpdate()
    {
        if (Grappling)
        {
            //Debug.Log(Vector3.Distance(end.position, transform.position));
        }
    }

    private void FixedUpdate()
    {
        if (Grappling && attached)
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
