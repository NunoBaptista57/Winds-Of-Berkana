using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ShipLevelManager : MonoBehaviour
{
    public static ShipLevelManager Instance { get; private set; }

    private int monoliths = 0;
    [SerializeField] bool HasMonoliths = false;
    [SerializeField] int requiredMonoliths = 0;
    [SerializeField] List<SplineAnimate> MonolithAnimations;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ActivateMonolith()
    {
        monoliths++;
        if (monoliths == requiredMonoliths)
        {
            foreach (var animation in MonolithAnimations)
            {
                animation.Play();
            }
        }
    }
}
