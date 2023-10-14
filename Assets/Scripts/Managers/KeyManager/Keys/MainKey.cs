using UnityEngine;

public class MainKey : MonoBehaviour, IKey
{
    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    private bool _collected = false;
    private Vector3 posOffset = new();
    private Vector3 tempPos = new();

    public void Collect()
    {
        _collected = true;
        gameObject.SetActive(false);
        ServiceLocator.instance.GetService<KeyManager>().UpdateValues();
    }

    public bool IsCollected()
    {
        return _collected;
    }

    public Transform GetPosition()
    {
        return transform;
    }

    private void Start()
    {
        posOffset = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void FixedUpdate()
    {
        //Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}