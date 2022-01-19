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
    [SerializeField] float Puzzle_distance;
    [SerializeField] float Light_intensity;
    [SerializeField] bool Light_Mechanic;
    [SerializeField] bool Movement_Mechanic;

    private Vector3 _centre;
    private float _angle;
    void Start()
    {
        foreach(PuzzlePiece _p in puzzle_piece)
        {
            _p.Collect += Collected;
        }
        _distance = new float[puzzle_piece.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Light_Mechanic)
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

            Debug.Log(distance_final);
            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", Light_intensity + distance_final * Puzzle_distance); //color = new Color(255 - distance * 5, 0, 98, 255);
        }

        if (Movement_Mechanic)
        {
            //gameObject.transform.localPosition = new Vector3(1f, 1.5f, 1f);
            Vector3 distance = (puzzle_piece[0].gameObject.transform.position - gameObject.transform.position).normalized;

            _angle += 1f * Time.deltaTime;

            var offset = new Vector3(Mathf.Sin(distance.x), 1.5f, Mathf.Cos(distance.z)) * 1;
            transform.localPosition = _centre + offset;
        }
    }

    private void Collected(int i)
    {
        Debug.Log("coleccionou peca: " + i);
        _distance[i] = 10000;
        gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", 1f);
    }
}
