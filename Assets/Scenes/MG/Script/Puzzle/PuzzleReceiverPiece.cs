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

    private PuzzleManager manager;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();
        defaultMaterial = rend.materials[0];
        manager = this.GetComponentInParent<PuzzleManager>();
    }


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

                manager.PieceReceived();


                return true;
              
            }
            return false;

        }
    }



  
}
