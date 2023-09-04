using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Scene[] _scenes;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public enum SceneType
    {

    }
}
