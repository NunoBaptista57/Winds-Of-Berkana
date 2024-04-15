using System.Collections;
using UnityEngine;

public class DoorEntrance : Door
{
    [SerializeField] private GameObject[] _doors;
    [SerializeField] private float _rotationSpeed = 1f;
     

    public override void ChangeDoorToOpen()
    {
        foreach (GameObject door in _doors)
        {
            door.SetActive(false);
        }
    }

    public override void PlayOpenDoorAnimation()
    {
        StartCoroutine(OpenDoors());
    }

private IEnumerator OpenDoors()
{
    Vector3 initialRotationLeft = _doors[0].transform.rotation.eulerAngles;
    Vector3 initialRotationRight = _doors[1].transform.rotation.eulerAngles;
    float rotationAmount = 0f;
    
    while (rotationAmount < 30f)
    {
        float rotationThisFrame = Time.deltaTime * _rotationSpeed;
        rotationAmount += rotationThisFrame;
        
        Quaternion newRotationLeft = Quaternion.Euler(initialRotationLeft.x, initialRotationLeft.y + rotationAmount, initialRotationLeft.z);
        Quaternion newRotationRight = Quaternion.Euler(initialRotationRight.x, initialRotationRight.y - rotationAmount, initialRotationRight.z);
        
        _doors[0].transform.rotation = newRotationLeft;
        _doors[1].transform.rotation = newRotationRight;
        
        yield return null;
    }
}

}