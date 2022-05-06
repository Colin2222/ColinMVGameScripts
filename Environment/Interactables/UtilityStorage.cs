using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityStorage : MonoBehaviour
{
    public Inventory inventory;
    public GameObject selection;
    public GameObject[] itemLocations;
    public int numColumns;
    public int numRows;

    [System.NonSerialized]
    public int currentItem = 0;

    private bool active = false;
    private bool insertionActive = false;
    private bool extractionActive = false;
    public GameObject arrow;

    void Start(){
        updateItemImages();
    }

    public void turnArrowOn(){
        if(arrow != null){
            arrow.SetActive(true);
        }
    }

    public void turnArrowOff(){
        if(arrow != null){
            arrow.SetActive(false);
        }
    }

    public void turnOn(){
        active = true;
        selection.SetActive(true);
        updateSelection();
    }

    public void turnOff(){
        active = false;
        selection.SetActive(false);
    }

    public void moveLeft(){
        if(currentItem != 0 && currentItem % numColumns != 0){
            currentItem--;
            updateSelection();
        }
    }

    public void moveRight(){
        if(currentItem != itemLocations.Length - 1 && (currentItem % numColumns) != 2){
            currentItem++;
            updateSelection();
        }
    }

    public void moveUp(){
        if(currentItem > numColumns - 1){
            currentItem = currentItem - numColumns;
            updateSelection();
        }
    }

    public void moveDown(){
        if(currentItem < itemLocations.Length - numColumns){
            currentItem = currentItem + numColumns;
            updateSelection();
        }
    }

    private void updateSelection(){
        selection.transform.position = itemLocations[currentItem].transform.position;
    }

    private void updateItemImage(int num){
        if(inventory.items[num] != null){
            itemLocations[num].gameObject.SetActive(true);
            itemLocations[num].GetComponent<SpriteRenderer>().sprite = inventory.items[num].sprite;
        } else{
            itemLocations[num].gameObject.SetActive(false);
        }
    }

    private void updateItemImages(){
        for(int i = 0; i < inventory.size; i++){
            if(inventory.items[i] != null){
                itemLocations[i].gameObject.SetActive(true);
                itemLocations[i].GetComponent<SpriteRenderer>().sprite = inventory.items[i].sprite;
            } else{
                itemLocations[i].gameObject.SetActive(false);
            }
        }
    }

    public void addToInventory(InventoryItem item, int place){
        if(inventory.items[place] == null){
            inventory.items[place] = item;
            updateItemImage(place);
        }
    }

    public InventoryItem removeFromUtility(){
        InventoryItem result = inventory.items[currentItem];
        inventory.items[currentItem] = null;
        updateItemImage(currentItem);
        return result;
    }
}
