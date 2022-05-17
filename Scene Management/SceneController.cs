using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public ItemManager itemManager;
    public CharacterManager characterManager;
    public DialogueManager dialogueManager;

    public float transitionTime;

    public string sceneName;

    public GameObject playerPrefab;

    [System.NonSerialized]
    public PlayerState playerState;
    public GameObject playerStateObject;
    GameObject playerStateObjectTest;
    GameObject playerObjectTest;
    [System.NonSerialized]
    public PlayerScript player;

    public GameObject dataManagerPrefab;
    public DataManager dataManager;
    GameObject dataManagerTest;

    [System.NonSerialized]
    public Vector3 spawnPosRight;
    [System.NonSerialized]
    public Vector3 spawnPosLeft;


    void Awake()
    {
        // check if there is a DontDestroyOnLoad DataManager, create a new one if there isnt
        dataManagerTest = GameObject.FindWithTag("DataManager");
        if(dataManagerTest == null){
            dataManager = Instantiate(dataManagerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<DataManager>();
        }
        else{
            dataManager = dataManagerTest.GetComponent<DataManager>();
        }

        // check if there is a DontDestroyOnLoad PlayerState, create a new one if there isnt
        playerStateObjectTest = GameObject.FindWithTag("PlayerState");
        if(playerStateObjectTest == null)
        {
            playerState = Instantiate(playerStateObject,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerState>();
        }
        else
        {
            playerState = playerStateObjectTest.GetComponent<PlayerState>();
        }

        // check if there is a DontDestroyOnLoad player, create a new one if there isnt
        playerObjectTest = GameObject.FindWithTag("PlayerTag");
        if(playerObjectTest == null){
            player = Instantiate(playerPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<PlayerScript>();
        }
        else{
            player = playerObjectTest.GetComponent<PlayerScript>();
        }

        // sync the DataManagers
        dataManager.findClockGUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets player position to the correct spawn position
        // PLACEHOLDER CODE, the player spawn position should depend on direction facing when last scene's transition was entered
        player.transform.position = spawnPosRight;

        // call player's inventory to find gem text UI element
        player.inventory.findGemText();

        // call players inventory manager to find the inventory UI element
        player.inventoryManager.inventoryUI = GameObject.FindWithTag("UIInventory").GetComponent<UIInventoryScript>();
        player.inventoryManager.setImages();
        player.inventory.changeGems(0);
    }

    // loads the scene of the inputted build index
    public IEnumerator SwitchScenes(int buildIndex, int entranceNumber, int directionNumber)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(buildIndex);

    }
}
