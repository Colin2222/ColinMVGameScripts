using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityChecker : MonoBehaviour
{
    [System.NonSerialized]
    public UtilityStorage currentShelf;
    [System.NonSerialized]
    public bool canUseUtility = false;

    void OnTriggerEnter2D(Collider2D utility){
        canUseUtility = true;
        currentShelf = utility.GetComponent<UtilityStorage>();
        currentShelf.turnArrowOn();
    }

    void OnTriggerExit2D(Collider2D utility){
        canUseUtility = false;
        currentShelf.turnArrowOff();
    }
}
