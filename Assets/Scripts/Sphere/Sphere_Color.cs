using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(puzzle_piece.transform.position, transform.position);
        Debug.Log(distance);
        gameObject.GetComponent<Renderer>().material.color = new Color(255 - distance*10, 0, 0, 1);
    }
}
