using System.Collections;
using System.Drawing.Text;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class DoorRightTower : Door
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _pointB;

    public override void ChangeDoorToOpen()
    {
        gameObject.SetActive(false);
    }

    public override void PlayOpenDoorAnimation()
    {
       StartCoroutine(SlideDoor());
    }

    private IEnumerator SlideDoor()
    {
        while (Vector3.Distance(transform.position, _pointB.position) >= 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}