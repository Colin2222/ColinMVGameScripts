using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPOIScript : MonoBehaviour
{
    public GameObject vcam;

    void OnTriggerEnter2D(Collider2D other){
        vcam.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other){
        vcam.SetActive(false);
    }
}
