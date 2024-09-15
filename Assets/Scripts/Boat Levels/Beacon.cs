using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    private bool _entered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _entered = true;
            Entered();
        }
    }

    public void Entered()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<BoatTutorialManager>().Next();
    }

    public void Appear()
    {
        gameObject.SetActive(true);
    }
}
