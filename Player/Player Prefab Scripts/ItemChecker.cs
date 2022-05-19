using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public PlayerScript playerScript;

    [System.NonSerialized]
    public bool canPickup = false;

    public Inventory inventory;

    private Item currentItem;

    void OnTriggerEnter2D(Collider2D other){
        canPickup = true;
        currentItem = other.GetComponent<Item>();
    }

    void OnTriggerExit2D(Collider2D other){
        canPickup = false;
    }

    public void pickUpItem(){
        if(currentItem.isGem){
            inventory.changeGems(1);
            currentItem.Pickup();
            Destroy(currentItem.transform.gameObject);
        }
        if(!currentItem.isGem){
            if(!inventory.isFull){
                int openIndex = 0;
                while(playerScript.inventoryManager.inventory.items[openIndex] != null){
                    openIndex++;
                }
                InventoryItem addition = Instantiate(currentItem.inventoryItem).GetComponent<InventoryItem>();
                inventory.addItem(addition, openIndex);
                addition.transform.parent = inventory.transform;
                currentItem.Pickup();
                Destroy(currentItem.transform.gameObject);
            }
        }
    }
}
