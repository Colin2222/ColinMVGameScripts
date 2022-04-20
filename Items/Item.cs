using System.Collections;
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

    public void Pickup() {
        sceneManager.itemManager.RemoveItem();

        // change saved data if unique item is picked up
        if(pickupCondition){
            sceneManager.dataManager.current.gameData[condition.savedName] = true;
        }
    }
}
