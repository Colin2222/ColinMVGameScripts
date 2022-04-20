using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.NonSerialized]
    public List<InventoryItem> items;
    [System.NonSerialized]
    public bool isFull;
    [System.NonSerialized]
    public int numItems = 0;

    public int size;

    public bool canHoldGems;
    public bool isPlayerInventory = false;
    private PlayerScript player;
    public int numGems;

    private Text gemText;

    void Start()
    {
        items = new List<InventoryItem>();
        if(isPlayerInventory){
            //gemText = GameObject.FindWithTag("UIGemBag").GetComponent<UIGemBag>().gemNumber;
            player = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerScript>();
        }

        // CHANGE THIS TO SYNC WITH PLAYERSTATE TO KEEP GEMS BETWEEN LEVELS
        changeGems(0);

        if(numItems == size){
            isFull = true;
        }
        else{
            isFull = false;
        }
    }

    public void changeGems(int num){
        numGems += num;
        if(gemText != null){
            gemText.text = numGems.ToString();
        }
    }

    public void addItem(InventoryItem item){
        if(numItems == size){

        }
        else{
            numItems++;
            items.Add(item);
        }

        if(numItems == size){
            isFull = true;
        }
        else{
            isFull = false;
        }

        if(isPlayerInventory){
            player.inventoryManager.setImages();
        }
    }

    public void findGemText(){
        if(gemText != null){
            gemText = GameObject.FindWithTag("UIGemBag").GetComponent<UIGemBag>().gemNumber;
        }
    }
}
