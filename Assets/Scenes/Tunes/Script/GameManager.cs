using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    int puzzle_collected = 0;
    [SerializeField] private PuzzlePiece[] _puzzlepieces;
    [SerializeField] private Camera _main_camera;

    public event Action PuzzleFinished;
    public event Action PieceCollected;
    void Start()
    {
        foreach(PuzzlePiece puzzle in _puzzlepieces)
        {
            puzzle.Collect += pieceCollected;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pieceCollected()
    {
        puzzle_collected += 1;
        PieceCollected?.Invoke();
        Debug.Log("puzzles:" + puzzle_collected);
        if(puzzle_collected == _puzzlepieces.Length)
        {
            PuzzleFinished?.Invoke();
        }
    }

    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    #endregion
}
