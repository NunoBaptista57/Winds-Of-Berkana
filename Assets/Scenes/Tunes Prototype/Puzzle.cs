using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Puzzle : MonoBehaviour
{
    [SerializeField] GameObject[] puzzlePieces;
    [SerializeField] GameObject puzzle;
    private int _piecesCollected = 0;

    private int _pieceSelected = 0;

    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.PieceCollected += PuzzleCollected;
    }

    void Update()
    {
        
    }

    public void PuzzleCollected()
    {
        puzzlePieces[_piecesCollected].gameObject.SetActive(true);
        _piecesCollected += 1;
    }

    public void RotatePiece(float rotation)
    {
        puzzlePieces[_pieceSelected].gameObject.transform.Rotate(0, 0, rotation);
    }

    public void SelectPiece()
    {
        if(_pieceSelected == puzzlePieces.Length - 1)
        {
            puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
            _pieceSelected = 0;
        }
        else
        {
            puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
            _pieceSelected += 1;
        }

        puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 255);
    }

    #region Singleton

    private static Puzzle _instance;
    public static Puzzle Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Puzzle>();
            return _instance;
        }
    }

    #endregion
}
