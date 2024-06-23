using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour
{
    [SerializeField] private GameObject gameobjectToDeactivate;
    [SerializeField] private GameObject gameobjectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShipLevelManager.Instance.ActivateMonolith();
            if (gameobjectToDeactivate != null)
            {
                gameobjectToDeactivate.SetActive(false);
            }
            if (gameobjectToActivate != null)
            {
                gameobjectToActivate.SetActive(true);
            }
        }
    }
}
