using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleReceiverPiece : InteractableZone
{

    private Material defaultMaterial;
    public Material completedMaterial;
    public string puzzleKey = "Piece1";

    private MeshRenderer rend;
    private bool completed = false;

    private VitralPuzzleManager manager;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();
        defaultMaterial = rend.materials[0];

        if(GameObject.Find("VitralManager") != null)
            manager = GameObject.Find("VitralManager").GetComponent<VitralPuzzleManager>();
    }


    // Main function to determine if the correct piece was inserted into the receiver 
    public override bool OnInteractionBegin(GameObject currentObject)
    {
        if (completed || currentObject == null) return false;

        else
        {
            if(currentObject.name == puzzleKey){

                completed = true;

                //How to change materials on runtime
                rend.materials = new Material[2]
                {
                    defaultMaterial,
                    completedMaterial
                };

                manager.PuzzleCollected();


                return true;
              
            }
            return false;

        }
    }



  
}
