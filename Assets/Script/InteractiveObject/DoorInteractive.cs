using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractive : InteractiveAction
{
    public Animator animator;
    BoxCollider doorCollider;

    private void Awake()
    {
        InteractiveUse += Play_This_Object;
        doorCollider = GetComponent<BoxCollider>();
    }
    void Play_This_Object(bool Action)
    {
        animator.SetBool("UseAction", Action);
        doorCollider.isTrigger = Action;
    }
}
