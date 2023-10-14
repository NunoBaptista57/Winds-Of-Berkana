using System.Collections.Generic;
using UnityEngine;

public class MovingTree : MonoBehaviour
{
    private bool _isOpen = false;

    public void Open(LevelState levelState)
    {
        if (!_isOpen && levelState == LevelState.BastionState_Puzzle2)
        {
            gameObject.GetComponent<Animator>().SetTrigger("Move");
            _isOpen = true;
        }
    }

    private void Start()
    {
        ServiceLocator.instance.GetService<Bastion1LevelManager>().OnLevelStateChanged += Open;
    }
}