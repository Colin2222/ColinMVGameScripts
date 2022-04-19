using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnHandler : MonoBehaviour
{
    public CharacterScript characterScript;
    [System.NonSerialized]
    public SceneController sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        // find the scenemanager
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();

        int highestPrecedence = 0;
        bool beingSpawned = false;
        Vector3 spawnLocation = new Vector3(0,0,0);
        // check each spot if it is valid, update if it has the highest precedence so far
        foreach(Transform child in transform){
            CharacterSpawnPosition currentPos = child.GetComponent<CharacterSpawnPosition>();
            if(currentPos != null){
                if(!currentPos.isTime){
                    if((sceneManager.dataManager.GetData(currentPos.savedName) && currentPos.mustBeTrue) ||
                        (!sceneManager.dataManager.GetData(currentPos.savedName) && !currentPos.mustBeTrue)){
                        if(currentPos.precedence > highestPrecedence){
                            highestPrecedence = currentPos.precedence;
                            if(currentPos.dontSpawn){
                                beingSpawned = false;
                            } else {
                                spawnLocation = currentPos.gameObject.transform.position;
                                beingSpawned = true;
                            }
                        }
                    }
                } else {
                    int currentTime = Mathf.FloorToInt(sceneManager.dataManager.currentTime);
                    if(currentTime >= int.Parse(currentPos.savedName.Substring(0,currentPos.savedName.IndexOf(':'))) &&
                        currentTime < int.Parse(currentPos.savedName.Substring(currentPos.savedName.IndexOf(':') + 1))){
                        if(currentPos.precedence > highestPrecedence){
                            highestPrecedence = currentPos.precedence;
                            if(currentPos.dontSpawn){
                                beingSpawned = false;
                            } else {
                                spawnLocation = currentPos.gameObject.transform.position;
                                beingSpawned = true;
                            }
                        }
                    }
                }
            }
        }

        // spawn character in right position or despawn based on highest precedence position that conditions were met for
        if(beingSpawned){
            characterScript.rigidbody2d.position = spawnLocation;
        } else {
            characterScript.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
