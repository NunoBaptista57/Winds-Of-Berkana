using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    public int value;

    void Start()
    {

    }

    void Update()
    {

    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("You collected the item");
            Destroy(gameObject);
            CoinCounter.instance.IncreaseCoins(value);
        }
    }

}
