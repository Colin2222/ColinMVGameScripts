﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private SceneController sceneManager;

    public bool isGem;
    public string id;
    public string displayName;
    public InventoryItem inventoryItem;
    public ItemSpawnCondition condition;
    public bool pickupCondition;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();
        GetComponent<SpriteRenderer>().sortingOrder = sceneManager.itemManager.AddItem();
    }

    void Pickup() {
        sceneManager.itemManager.RemoveItem();

        // change saved data if unique item is picked up
        if(pickupCondition){
            sceneManager.dataManager.current.gameData[condition.savedName] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Inventory destination = other.GetComponent<Inventory>();
        if(destination != null){
            if(isGem){
                destination.changeGems(1);
                Pickup();
                Destroy(gameObject);
            }
            if(!isGem){
                if(!destination.isFull){
                    destination.addItem(inventoryItem);
                    Pickup();
                    Destroy(gameObject);
                }

            }
        }
    }
}
