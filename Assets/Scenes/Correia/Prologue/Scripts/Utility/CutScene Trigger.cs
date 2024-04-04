using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerTimelineController : MonoBehaviour
{
    [Header("Optional Reference")]
    [SerializeField] CutSceneManager cutSceneManager;

    [SerializeField] private Vector3 startingPosition;
    private bool isMovingToStartPosition = false;
    private Transform shipTransform;
    [SerializeField] private Transform moveTarget;
    private Transform camTransform;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 0.0002f;
    [SerializeField] private float stoppingDistance = 0.1f;

void Update()
    {
        if (isMovingToStartPosition)
        {
            // Move the ship
            shipTransform.gameObject.GetComponent<BoatMovement>().canMove = false;
            Vector3 shipPosition = shipTransform.position;
            shipPosition.y = moveTarget.position.y;
            float distance = Vector3.Distance(shipPosition, moveTarget.position);
            if (distance > stoppingDistance)
            {
                Vector3 direction = (moveTarget.position - shipTransform.position).normalized;
                direction.y = 0;
                Vector3 newPosition = shipTransform.position + direction * moveSpeed * Time.deltaTime;
                shipTransform.position = newPosition;

                // Rotate the ship towards the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                shipTransform.rotation = Quaternion.Lerp(shipTransform.rotation, targetRotation, rotationSpeed);
            }
            else
            {
                isMovingToStartPosition = false;
                cutSceneManager.beginCutScene();
                shipTransform.gameObject.GetComponent<BoatMovement>().canMove = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        BoatMovement boatMovement = other.GetComponent<BoatMovement>();
        if (boatMovement != null)
        {
            boatMovement.AllowPlayerControl(false);
            moveSpeed = boatMovement.currentSpeed;
            shipTransform = other.gameObject.transform;
            isMovingToStartPosition = true;
        }
    }
}
