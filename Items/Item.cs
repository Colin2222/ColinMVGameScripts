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
    public bool inScene;
    public string prefabName;

    // Start is called before the first frame update
    void Start()
    {
        if(inScene){
            transform.gameObject.SetActive(false);
        }
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();

        // get order within sorting layer to prevent the weird clipping of sprites when the camera moves
        GetComponent<SpriteRenderer>().sortingOrder = sceneManager.itemManager.AddItem();
    }

    public void Pickup() {
        sceneManager.itemManager.RemoveItem();
    }
}
