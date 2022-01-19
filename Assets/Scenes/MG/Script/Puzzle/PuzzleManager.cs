using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public float totalPieces = 3;

    public float currentPiecesReceived = 0;

    public Animator movableWall;

    public void PieceReceived()
    {
        this.currentPiecesReceived++;

        if(currentPiecesReceived == totalPieces)
        {
            FinishPuzzle();
        }
    }

    private void FinishPuzzle()
    {
        movableWall.SetTrigger("Move");
    }


}
