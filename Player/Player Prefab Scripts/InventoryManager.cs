using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    [System.NonSerialized]
    public UIInventoryScript inventoryUI;

    [System.NonSerialized]
    public int currentItem = 0;
    [System.NonSerialized]
    public bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GameObject.FindWithTag("UIInventory").GetComponent<UIInventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setImages(){
        for(int i = 0; i < inventory.size; i++){
            if(inventory.items[i] != null){
                inventoryUI.images[i].sprite = inventory.items[i].sprite;
                inventoryUI.images[i].gameObject.SetActive(true);
            } else{
                inventoryUI.images[i].gameObject.SetActive(false);
            }
        }
    }

    public void turnOn(){
        active = true;
        inventoryUI.selection.gameObject.SetActive(true);
        inventoryUI.selection.gameObject.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                                        inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void turnOff(){
        active = false;
        inventoryUI.selection.gameObject.SetActive(false);
    }

    public void moveSelectionRight(){
        if(currentItem < inventory.size - 1){
            currentItem += 1;
            inventoryUI.selection.gameObject.GetComponent<RectTransform>().anchoredPosition =
                                    new Vector2(inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                                                inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    public void moveSelectionLeft(){
        if(currentItem > 0){
            currentItem -= 1;
            inventoryUI.selection.gameObject.GetComponent<RectTransform>().anchoredPosition =
                                    new Vector2(inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                                                inventoryUI.images[currentItem].gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    public void dropItem(){
        inventory.dropItem(currentItem);
        setImages();
    }

    public InventoryItem removeItem(int num){
        InventoryItem result = inventory.removeItem(num);
        setImages();
        return result;
    }
}
