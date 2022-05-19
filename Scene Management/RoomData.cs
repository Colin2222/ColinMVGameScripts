using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public string roomDataFile;

    [System.NonSerialized]
    public XmlDocument roomXml;
    [System.NonSerialized]
    public XmlNode currentNode;
    [System.NonSerialized]
    public XmlNode currentParent;

    void Awake(){
        // create new xmldocument to read
        roomXml = new XmlDocument();
        roomXml.Load("Assets/Resources/RoomData/" + roomDataFile + ".xml");

        // instantiate all items saved in the xml file
        currentParent = roomXml.FirstChild.NextSibling.FirstChild;
        for(int i = 0; i < currentParent.ChildNodes.Count; i++){
            XmlNode item = currentParent.ChildNodes[i];
            string currentPrefab = item.Attributes["prefab"].Value;
            float currentX = float.Parse(item.Attributes["x"].Value);
            float currentY = float.Parse(item.Attributes["y"].Value);
            GameObject currentItem = Instantiate(Resources.Load("Assets/Prefabs/Items/" + currentPrefab, typeof(GameObject)), new Vector3(currentX, currentY, 0), Quaternion.identity) as GameObject;
        }

        // populate utility inventories with contents saved in the xml file
        currentParent = currentParent.NextSibling;
        List<GameObject> utilities = new List<GameObject>(GameObject.FindGameObjectsWithTag("Utility"));
        for(int i = 0; i < currentParent.ChildNodes.Count; i++){
            // find the corresponding utility inventory for the current node
            bool utilityFound = false;
            int j = 0;
            while(!utilityFound){
                if(utilities[j].GetComponent<UtilityStorage>().roomDataId == int.Parse(currentParent.ChildNodes[i].Attributes["number"].Value)){
                    Debug.Log("UTILITY FOUND");
                    utilityFound = true;
                    UtilityStorage currentUtility = utilities[j].GetComponent<UtilityStorage>();
                    currentNode = currentParent.ChildNodes[i];
                    for(int k = 0; k < currentNode.ChildNodes.Count; k++){
                        string currentPrefab = currentNode.ChildNodes[k].Attributes["prefab"].Value;
                        GameObject currentItem = (Instantiate(Resources.Load("Assets/Prefabs/Items/" + currentPrefab, typeof(GameObject))) as GameObject);
                        InventoryItem invenItem = currentItem.GetComponent<InventoryItem>();
                        currentUtility.addToInventory(invenItem, int.Parse(currentNode.ChildNodes[k].Attributes["location"].Value));
                    }
                } else{
                    j++;
                }
            }
        }
    }

    // writes the data in the state lists to the xml file and closes
    public void WriteToXML(){
        // write items to xml file
        currentParent = roomXml.FirstChild.NextSibling.FirstChild;
        currentParent.RemoveAll();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        for(int i = 0; i < items.Length; i++){
            // create new element for item and populate with appropriate attributes
            XmlElement currentItem = roomXml.CreateElement("item");
            XmlAttribute prefabAttribute = roomXml.CreateAttribute("prefab");
            XmlAttribute xAttribute = roomXml.CreateAttribute("x");
            XmlAttribute yAttribute = roomXml.CreateAttribute("y");

            prefabAttribute.Value = items[i].GetComponent<Item>().prefabName;
            currentItem.Attributes.Append(prefabAttribute);

            xAttribute.Value = items[i].transform.position.x.ToString();
            currentItem.Attributes.Append(xAttribute);

            yAttribute.Value = items[i].transform.position.y.ToString();
            currentItem.Attributes.Append(yAttribute);

            // append new item xml element to the parent element
            currentParent.AppendChild(currentItem);
        }

        // write utility inventories to xml file
        currentParent = currentParent.NextSibling;
        currentParent.RemoveAll();
        GameObject[] utilities = GameObject.FindGameObjectsWithTag("Utility");
        for(int i = 0; i < utilities.Length; i++){
            // get the relevant utility being written to xml file
            UtilityStorage currentUtility = utilities[i].GetComponent<UtilityStorage>();

            // create element for utility and populate it with appropriate number attribute
            XmlElement currentUtilityElement = roomXml.CreateElement("utility");
            XmlAttribute numberAttribute = roomXml.CreateAttribute("number");
            numberAttribute.Value = currentUtility.roomDataId.ToString();
            currentUtilityElement.Attributes.Append(numberAttribute);

            for(int j = 0; j < currentUtility.inventory.size; j++){
                if(currentUtility.inventory.items[j] != null){
                    // since inventoryitem was detected, create and append an xml element for it
                    XmlElement currentItem = roomXml.CreateElement("inventoryItem");
                    XmlAttribute prefabAttribute = roomXml.CreateAttribute("prefab");
                    prefabAttribute.Value = currentUtility.inventory.items[j].prefabName;
                    XmlAttribute locationAttribute = roomXml.CreateAttribute("location");
                    locationAttribute.Value = j.ToString();
                    currentItem.Attributes.Append(prefabAttribute);
                    currentItem.Attributes.Append(locationAttribute);
                    currentUtilityElement.AppendChild(currentItem);
                }
            }

            currentParent.AppendChild(currentUtilityElement);
        }

        // permanently saves data to xml file
        roomXml.Save("Assets/Resources/RoomData/" + roomDataFile + ".xml");
    }
}
