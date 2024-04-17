using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;


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
    [SerializeField] private float stoppingDistance = 0.12f;



    [SerializeField] private bool needsPlayerProximity = true;
    [SerializeField] private SplineContainer path;
    public float offsetRotationX = 10f; // Visual only Rotation fix

    [Header("References")]
    public BoatMovement playerBoatMovement;

    private Spline currentSpline;
    private Rigidbody rb;
    private Transform playerPos;
    private int i = 0;
    private float t = 0f;


    public void HitJunction(Spline path)
    {
        currentSpline = path;
    }

    private void Start()
    {
        playerPos = playerBoatMovement.transform;

        rb = GetComponent<Rigidbody>();

        currentSpline = path.Splines[0];
    }

    void Update()
    {
        if (isMovingToStartPosition)
        {
            // Move the ship
            shipTransform.gameObject.GetComponent<BoatMovement>().canMove = false;
            Vector3 shipPosition = shipTransform.position;
            shipPosition.y = currentSpline.Knots.Last().Position.y;
            float distance = Vector3.Distance(shipPosition, currentSpline.Knots.Last().Position);
            
            if (distance > stoppingDistance)
            {
                var native = new NativeSpline(currentSpline);

                Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));

                Vector3 up = native.EvaluateUpVector(t);

                var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0)));

                shipTransform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

                shipTransform.rotation *= Quaternion.Euler(offsetRotationX, 0, 0);

                shipTransform.position += forward * moveSpeed * Time.deltaTime;

                t += moveSpeed * Time.deltaTime / currentSpline.GetLength();
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
            BezierKnot firstKnot = currentSpline.Knots.First();
            print(firstKnot.Position);
            firstKnot.Position = shipTransform.position;
            firstKnot.Rotation = shipTransform.rotation;
            currentSpline.SetKnot(0, firstKnot);
        }
    }
}
