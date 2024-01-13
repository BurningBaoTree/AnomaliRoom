using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractive : InteractiveAction
{
    public Animator animator;
    BoxCollider doorCollider;
    public Transform wellcomMatt;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wellcomMatt.position + Vector3.forward*0.2f, new Vector3(1,0.1f,0.5f));
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wellcomMatt.position - Vector3.forward * 0.2f, new Vector3(1, 0.1f, 0.5f));
    } 
#endif

}
