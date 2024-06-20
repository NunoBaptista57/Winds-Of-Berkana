using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _distance = 10f;
    [SerializeField] private float _speed = 1f;
    public Vector3 Velocity { get; private set; }
    private Vector3 _origin;
    private Vector3 _direction;
    private Vector3 _lastPosition;
    private MeshCollider _collider;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _origin + _direction * _distance, _speed * Time.deltaTime);
        transform.Rotate(_speed * Time.deltaTime * new Vector3(0, 5, 0));
        Velocity = transform.position - _lastPosition;
        _lastPosition = transform.position;
        if (Vector3.Distance(_origin + _direction * _distance, transform.position) < 0.001f)
        {
            _origin = transform.position;
            _direction *= -1;
        }
    }

    private void Start()
    {
        _origin = transform.position;
        _direction = transform.forward;
        Velocity = Vector3.zero;
        _lastPosition = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterManager characterManager))
        {
            // characterManager.Push(Velocity.normalized * 0.00000001f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }

    private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
    }
}