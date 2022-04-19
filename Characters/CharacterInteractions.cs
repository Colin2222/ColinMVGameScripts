using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractions : MonoBehaviour
{
    [System.NonSerialized]
    public bool talking = false;
    private bool silent = false;

    public Dialogue dialogue;
    Dialogue currentDialogue;

    private PlayerMover player;

    public CharacterScript characterScript;
    [System.NonSerialized]
    public DataManager gameData;
    [System.NonSerialized]
    public DialogueManager manager;

    void Start()
    {
        gameData = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        manager = FindObjectOfType<DialogueManager>();
    }

    void Update(){
        if(!(manager.isSilence) && silent){
            if(player != null){
                Interact(player);
            }
        }

        if(manager.isSilence){
            silent = true;
        }
        else{
            silent = false;
        }
    }

    public void Interact(PlayerMover player)
    {
        this.player = player;
        if(!manager.isTalking)
        {
            if(dialogue != null){
                talking = manager.StartDialogue(dialogue, player);
            }
        }
        else
        {

            if(!manager.isSilence){
                talking = manager.HandleNextElement();
            }

        }
    }
}
