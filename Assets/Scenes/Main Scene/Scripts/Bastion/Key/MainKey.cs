using UnityEngine;

public class MainKey : MonoBehaviour, IKey
{
    [SerializeField] private EventSender _eventSender;
    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    private Vector3 posOffset = new();
    private Vector3 tempPos = new();
    private bool _collected = false;

    void Start()
    {
        posOffset = transform.position;
    }

    public void Collect()
    {
        _collected = true;
        gameObject.SetActive(false);
        _eventSender.InvokeCollectedKeyEvent();
    }

    public bool IsCollected()
    {
        return _collected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    void FixedUpdate()
    {
        //Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}