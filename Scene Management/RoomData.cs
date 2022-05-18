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

    void Awake(){
        // create new xmldocument to read
        roomXml = new XmlDocument();
        roomXml.Load("Assets/Resources/RoomData/" + roomDataFile + ".xml");

        // instantiate all items saved in the xml file
        currentNode = roomXml.FirstChild.NextSibling;
        for(int i = 0; i < currentNode.ChildNodes.Count; i++){
            XmlNode item = currentNode.ChildNodes[i];
            string currentPrefab = item.Attributes["prefab"].Value;
            float currentX = float.Parse(item.Attributes["x"].Value);
            float currentY = float.Parse(item.Attributes["y"].Value);
            GameObject currentItem = Instantiate(Resources.Load("Assets/Prefabs/Items/" + currentPrefab, typeof(GameObject)), new Vector3(currentX, currentY, 0), Quaternion.identity) as GameObject;
        }
    }

    // writes the data in the state lists to the xml file and closes
    public void WriteToXML(){
        currentNode = roomXml.FirstChild.NextSibling;
        currentNode.RemoveAll();
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
            currentNode.AppendChild(currentItem);
        }

        // permanently saves data to xml file
        roomXml.Save("Assets/Resources/RoomData/" + roomDataFile + ".xml");
    }
}
