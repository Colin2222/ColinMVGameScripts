using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour
{
    [System.NonSerialized]
    public bool canEnterDoor = false;
    private DoorTransitionScript transition;

    void OnTriggerEnter2D(Collider2D door){
        canEnterDoor = true;
        transition = door.GetComponent<DoorTransitionScript>();
    }

    void OnTriggerExit2D(Collider2D door){
        canEnterDoor = false;
    }

    public void triggerTransition(){
        transition.transitionThroughDoor();
    }
}
