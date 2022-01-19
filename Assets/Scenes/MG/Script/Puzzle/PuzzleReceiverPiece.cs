using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleReceiverPiece : InteractableZone
{

    private Material defaultMaterial;
    public Material completedMaterial;
    public string puzzleKey = "PuzzlePiece1";

    private MeshRenderer rend;
    private bool completed = false;
    public Animator movableWall;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();
        defaultMaterial = rend.materials[0];
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

                //Open a door
                movableWall.SetTrigger("Move");
                
                return true;
              
            }
            return false;

        }
    }



  
}
