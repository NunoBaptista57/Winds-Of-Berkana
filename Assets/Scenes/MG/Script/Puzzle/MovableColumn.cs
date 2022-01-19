using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableColumn : InteractableItem
{
    Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public override void OnInteractionBegin(Transform playerTransform)
    {
        animator.SetTrigger("Move");
    }

    public override void OnInteractionEnd()
    {

    }
}
