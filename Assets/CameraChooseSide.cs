using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChooseSide : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] float AngularSpeedThreshold = 10;
    [SerializeField] float SideExponent = 2;
    [SerializeField] float LerpStrength = 1;
    [SerializeField] float MinVelocityLerp = 0.1f;
    [SerializeField] float MinSide = 0.1f;
    Cinemachine3rdPersonFollow cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    // Update is called once per frame
    void Update()
    {

        float side = target.angularVelocity.y / AngularSpeedThreshold;
        side = Mathf.Clamp01(FloatUtils.Intensify(Mathf.Sign(side) * Mathf.Max(MinSide, Mathf.Abs(side)), SideExponent) / 2 + 0.5f);
        cameraFollow.CameraSide = Mathf.Lerp(cameraFollow.CameraSide, side, (Mathf.Max(Mathf.Clamp01(Mathf.Abs(target.angularVelocity.y) / AngularSpeedThreshold), MinVelocityLerp)) * LerpStrength * Time.deltaTime);
    }
}
