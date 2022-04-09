using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Color : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] puzzle_piece;// = GameObject.FindGameObjectWithTag("Puzzle_Piece");

    [Header("Light Variables")]
    [SerializeField] float Light_intensity;
    [SerializeField] bool Light_Mechanic;
    [SerializeField] bool Movement_Mechanic;

    private Vector3 _centre;

    private GameObject closestSphere;

    private float _angle;

    void Start()
    {
        _centre = this.transform.parent.position;
        GetClosestSphere();
    }

    // Change this to check which sphere was caught
    public void NextSphere()
    {
        GetClosestSphere();
    }

    // Get Closest Sphere from the List
    public void GetClosestSphere()
    {
        float maxDistance = float.MaxValue;

        foreach(var p in puzzle_piece)
        {
            if (p.activeSelf)
            {
                var distance = Vector3.Distance(p.transform.position, transform.position);

                if(distance < maxDistance)
                {
                    closestSphere = p;
                }
            }
          


        }

    }

    // Update is called once per frame
    void Update()
    {

        
        // Get Distance
        var currentDistance = Vector3.Distance(closestSphere.transform.position, this.transform.position);

        // Invert it
        var invertedDistance = 1 / currentDistance;

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
            /*distance_final = 10000;
            for (int i = 0; i < puzzle_piece.Length; i++)
            {
                if (puzzle_piece[i].gameObject.activeSelf)
                {
                    _distance[i] = Vector3.Distance(puzzle_piece[i].gameObject.transform.position, transform.position);
                    distance_final = Mathf.Min(distance_final, _distance[i]);
                }
            }

            gameObject.GetComponent<Renderer>().material.SetFloat("_EmissiveExposureWeight", Light_intensity + distance_final * Puzzle_distance); //color = new Color(255 - distance * 5, 0, 98, 255);*/

           
            var actualIntensity = Light_intensity * invertedDistance;
          //  Debug.Log(actualIntensity);
            gameObject.GetComponent<Renderer>().material.SetFloat("Emissive Intensity", actualIntensity);

           Debug.Log(" Intensity: " + gameObject.GetComponent<Renderer>().material.GetFloat("Emissive Intensity"));
         //   Debug.Log(" HasPropery: " + gameObject.GetComponent<Renderer>().material.HasProperty("_EmissionIntensity"));
        }

        if (Movement_Mechanic)
        {
            //gameObject.transform.localPosition = new Vector3(1f, 1.5f, 1f);
            Vector3 distance = (closestSphere.transform.position - this.transform.parent.position).normalized;

            _angle += 1f * Time.deltaTime;

            var offset = new Vector3(distance.x, 1.5f, distance.z);
            transform.position = Vector3.Lerp(this.transform.parent.position, closestSphere.transform.position, 0.01f);
            transform.localPosition = new Vector3(transform.localPosition.x, 1.5f, transform.localPosition.z);
        }
    }


}
