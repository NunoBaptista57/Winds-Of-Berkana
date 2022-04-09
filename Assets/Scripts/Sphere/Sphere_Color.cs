using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");
    private float[] _distance;
    private float distance_final;

    [Header("Light Variables")]
    [SerializeField] float Puzzle_distance;
    [SerializeField] float Light_intensity;
    [SerializeField] bool Light_Mechanic;
    [SerializeField] bool Movement_Mechanic;

    private Vector3 _centre;
    
    public int currentPuzzleIndex = 0;
    private float _angle;

    void Start()
    {
        _distance = new float[puzzle_piece.Length];

        _centre = this.transform.parent.position;
    }

    // Change this to check which sphere was caught
    public void NextSphere()
    {
        currentPuzzleIndex += 1;
    }

    // Get Closest Sphere from the List
    public void ClosestSphere()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        distance_final = 10000;
        int closest_sphere = 0;

        // Need to check for closest afterwards
        _distance[currentPuzzleIndex] = Vector3.Distance(puzzle_piece[currentPuzzleIndex].gameObject.transform.position, transform.position);

     /*   for (int i = 0; i < puzzle_piece.Length; i++)
        {
            if (puzzle_piece[i].activeSelf)
            {
                _distance[i] = Vector3.Distance(puzzle_piece[i].gameObject.transform.position, transform.position);
                if(_distance[i] < distance_final)
                {
                    closest_sphere = i;
                    distance_final = Mathf.Min(distance_final, _distance[i]);
                }
            }
        }*/

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

            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", Light_intensity + distance_final * Puzzle_distance); //color = new Color(255 - distance * 5, 0, 98, 255);
        }

        if (Movement_Mechanic)
        {
            //gameObject.transform.localPosition = new Vector3(1f, 1.5f, 1f);
            Vector3 distance = (puzzle_piece[closest_sphere].gameObject.transform.position - this.transform.parent.position).normalized;

            _angle += 1f * Time.deltaTime;

            var offset = new Vector3(distance.x, 1.5f, distance.z);
            transform.position = Vector3.Lerp(this.transform.parent.position, puzzle_piece[closest_sphere].gameObject.transform.position, 0.01f);
            transform.localPosition = new Vector3(transform.localPosition.x, 1.5f, transform.localPosition.z);
        }
    }


}
