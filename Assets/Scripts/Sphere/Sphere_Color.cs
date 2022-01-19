using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PuzzlePiece[] puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");
    private float[] _distance;
    private float distance_final;
    void Start()
    {
        puzzle_piece[0].Collect += Collected;
        puzzle_piece[1].Collect += Collected;
        _distance = new float[puzzle_piece.Length];
    }

    // Update is called once per frame
    void Update()
    {
        distance_final = 10000;
        for (int i = 0; i < puzzle_piece.Length; i++)
        {
            if (puzzle_piece[i].gameObject.activeSelf)
            {
                _distance[i] = Vector3.Distance(puzzle_piece[i].gameObject.transform.position, transform.position);
                distance_final = Mathf.Min(distance_final, _distance[i]);
            }
        }

        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", -5f + distance_final * 0.1f); //color = new Color(255 - distance * 5, 0, 98, 255);
        
    }

    private void Collected(int i)
    {
        Debug.Log("coleccionou peca: " + i);
        _distance[i] = 10000;
        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1f);
    }
}
