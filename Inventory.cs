using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [System.NonSerialized]
    public InventoryItem[] items;
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

    void Awake(){
        items = new InventoryItem[size];
        for(int i = 0; i < size; i++){
            items[i] = null;
        }
    }

    void Start()
    {
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

    public void addItem(InventoryItem item, int num){
        if(items[num] == null){
            numItems++;
            items[num] = item;
            if(numItems == size){
                isFull = true;
            }
        }

        if(isPlayerInventory){
            player.inventoryManager.setImages();
        }
    }

    public void addItem(InventoryItem insertion){
        int openIndex = 0;
        while(items[openIndex] != null){
            openIndex++;
        }

        items[openIndex] = insertion;
    }

    public void findGemText(){
        if(gemText != null){
            gemText = GameObject.FindWithTag("UIGemBag").GetComponent<UIGemBag>().gemNumber;
        }
    }

    public void dropItem(int num){
        if(items[num] != null){
            Instantiate(items[num].prefab, transform.position, Quaternion.identity);
            numItems--;
            isFull = false;
            Destroy(items[num].transform.gameObject);
            items[num] = null;
        }
    }

    public InventoryItem removeItem(int num){
        if(items[num] != null){
            numItems--;
            isFull = false;
            InventoryItem result = items[num];
            items[num] = null;
            return result;
        }
        return null;
    }
}
