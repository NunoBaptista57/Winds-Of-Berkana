using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlock : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            foreach (Transform child in this.transform.parent)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
