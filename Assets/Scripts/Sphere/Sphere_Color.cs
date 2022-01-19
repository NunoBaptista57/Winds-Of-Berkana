using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PuzzlePiece puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");
    private bool puzzle_exists = true;
    void Start()
    {
        puzzle_piece.Collect += Collected;
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzle_exists == true)
        {
            float distance = Vector3.Distance(puzzle_piece.gameObject.transform.position, transform.position);
            Debug.Log(distance);
            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", -5f + distance * 0.1f); //color = new Color(255 - distance * 5, 0, 98, 255);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1f);
        }
    }

    private void Collected()
    {
        puzzle_exists = false;
    }
}
