using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionScript : MonoBehaviour
{
    public int buildIndex = 0;

    public int entranceNumber = 0;

    public int entranceDirection = 0;

    bool active = false;
    public GameObject arrow;
    public Transform doorTransform;
    PlayerScript player;

    public SceneController sceneManager;

    void Awake()
    {
        if(sceneManager.playerState.entranceNumber == entranceNumber)
        {
            sceneManager.spawnPosRight = doorTransform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        active = true;
        arrow.SetActive(true);
        player = other.transform.parent.GetComponent<PlayerScript>();
        /*
        PlayerScript player = other.transform.parent.GetComponent<PlayerScript>();
        sceneManager.playerState.entranceNumber = entranceNumber;
        if(player != null && !(player.isSpawning))
        {

            StartCoroutine(sceneManager.SwitchScenes(buildIndex,entranceNumber,entranceDirection));
            sceneManager.player.resetSpawning();

        }
        */
    }

    void OnTriggerExit2D(Collider2D player){
        active = false;
        arrow.SetActive(false);
    }

    public void transitionThroughDoor(){
        sceneManager.playerState.entranceNumber = entranceNumber;
        if(!(player.isSpawning)){
            // save room data to xml
            sceneManager.roomData.WriteToXML();

            // start loading of new scene
            StartCoroutine(sceneManager.SwitchScenes(buildIndex,entranceNumber,entranceDirection));
            sceneManager.player.resetSpawning();
        }
    }
}
