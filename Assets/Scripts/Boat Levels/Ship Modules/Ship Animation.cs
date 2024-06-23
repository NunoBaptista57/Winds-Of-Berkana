using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimation : MonoBehaviour
{
    private BoatMovement _BoatMovement;

    [Header("Tilting")]
    [SerializeField] Transform visualModelTransform;
    [SerializeField] Transform Sail_L;
    [SerializeField] Transform Sail_R;
    [SerializeField, Range(0, 90)] float maxTiltingAngle = 10;

    void Awake()
    {
        _BoatMovement = gameObject.GetComponent<BoatMovement>();
    }

    void Update()
    {
        // Tilting
        if (visualModelTransform != null)
        {
            Vector3 currentEulerAngles = visualModelTransform.rotation.eulerAngles;
            float tiltAngle = maxTiltingAngle * -_BoatMovement.input.Turn;
            currentEulerAngles.z = tiltAngle;
            Quaternion targetRotation = Quaternion.Euler(currentEulerAngles);
            visualModelTransform.rotation = Quaternion.Lerp(visualModelTransform.rotation, targetRotation, Time.fixedDeltaTime * 2.5f);
        }

        /*if (Sail_L != null && Sail_R != null)
        {
            float currentSpeed = _BoatMovement.currentSpeed;
            float maxVelocity = _BoatMovement.MaxVelocity;

            if (maxVelocity > 0)
            {
                float scaleFactor = Mathf.Lerp(0.1f, 1.0f, currentSpeed / maxVelocity);

                print(scaleFactor);

                Sail_L.localScale = new Vector3(Sail_L.localScale.x, Sail_L.localScale.y * scaleFactor, Sail_L.localScale.z);
                Sail_R.localScale = new Vector3(Sail_R.localScale.x, Sail_R.localScale.y * scaleFactor, Sail_R.localScale.z);
            }
        }*/
    }
}
